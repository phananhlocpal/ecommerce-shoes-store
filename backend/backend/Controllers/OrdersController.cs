using AutoMapper;
using backend.Dtos;
using backend.Entities;
using backend.Models;
using backend.Repositories.EntitiesRepo;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IRepo<Order> _orderRepo;
        private readonly IRepo<OrderItem> _orderItemRepo;
        private readonly CartItemRepo _cartItemRepo;
        private readonly ILogger<OrdersController> _logger;
        private readonly VNPayService _vnPayService;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly OrderRepo _orderRepoSingle;

        public OrdersController(IRepo<Order> orderRepo, IRepo<OrderItem> orderItemRepo,  CartItemRepo cartItemRepo, ILogger<OrdersController> logger, VNPayService vNPayService, IDistributedCache distributedCache, IConfiguration config, IMapper mapper, OrderRepo orderRepoSingle)
        {
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _cartItemRepo = cartItemRepo;
            _logger = logger;
            _vnPayService = vNPayService;
            _distributedCache = distributedCache;
            _config = config;
            _mapper = mapper;
            _orderRepoSingle = orderRepoSingle;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetOrders()
        {
            var orders = await _orderRepo.GetAllAsync();
            var ordersDto = _mapper.Map<IEnumerable<OrderReadDto>>(orders);
            return Ok(ordersDto);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            var orderDto = _mapper.Map<OrderReadDto>(order);
            return Ok(orderDto);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetOrdersByCustomerId(int customerId)
        {
            var orders = await _orderRepoSingle.GetByConditionAsync(o => o.CustomerID == customerId);
            var ordersDto = _mapper.Map<IEnumerable<OrderReadDto>>(orders);
            return Ok(ordersDto);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderUpdateDto order)
        {
            var existingOrder = await _orderRepo.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            var orderEntity = _mapper.Map(order, existingOrder);
            bool updated = await _orderRepo.UpdateAsync(orderEntity);
            if (!updated)
            {
                return BadRequest("Unable to update the order.");
            }

            return NoContent();
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            bool deleted = await _orderRepo.DeleteAsync(id);
            if (!deleted)
            {
                return BadRequest("Unable to delete the order.");
            }

            return NoContent();
        }

        // POST: api/Orders/paypal
        [HttpPost("paypal")]
        public async Task<ActionResult<Order>> PostOrderPayPal(OrderCreateDto order)
        {
            if (order == null)
            {
                return BadRequest("Invalid order data.");
            }
            var invoiceCreate = _mapper.Map<OrderCreateDto, Order>(order);
            invoiceCreate.Status = OrderStatus.Pending;
            invoiceCreate.DateTime = DateTime.Now;

            var createdInvoice = await _orderRepo.AddAsync(invoiceCreate);
            _logger.LogInformation("Invoice created successfully. Invoice ID: {InvoiceId}", createdInvoice.OrderID);

            // Get cart items by customerId
            var cartItems = await _cartItemRepo.GetCardItemByCustomerId(invoiceCreate.CustomerID);
            if (cartItems == null || !cartItems.Any())
            {
                _logger.LogError("No cart items found for customer ID: {CartID}", invoiceCreate.CustomerID);
                return BadRequest("No cart items found.");
            }

            var orderItems = cartItems.Select(cartItem => new OrderItem
            {
                Quantity = cartItem.Quantity,
                ProductSizeID = cartItem.ProductSizeID.Value,
                OrderID = createdInvoice.OrderID,
            }).ToList();

            foreach (var orderItem in orderItems)
            {
                await _orderItemRepo.AddAsync(orderItem);
            }
            _logger.LogInformation("Order items added successfully!");

            // Xóa giỏ hàng sau khi tạo đơn hàng thành công
            await _cartItemRepo.ClearCartAsync(invoiceCreate.CustomerID);

            return CreatedAtAction("GetOrder", new { id = createdInvoice.OrderID }, createdInvoice);
        }

        // POST: api/Orders/vnpay
        [HttpPost("vnpay")]
        public async Task<IActionResult> PostOrderVNPay([FromBody] VnPaymentRequestModel paymentRequest)
        {
            if (paymentRequest == null)
            {
                return BadRequest("Invalid payment request data.");
            }
            var order = paymentRequest.Order;

            await SetInvoiceToCacheAsync(order);

            try
            {
                var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, paymentRequest);

                if (string.IsNullOrEmpty(paymentUrl))
                {
                    return StatusCode(500, "Unable to generate payment URL.");
                }

                return Ok(new { redirectUrl = paymentUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the payment. Request: {Request}", JsonSerializer.Serialize(paymentRequest));
                return StatusCode(500, "An error occurred while creating the payment.");
            }
        }

        [HttpGet("paymentReturn")]
        public async Task<IActionResult> PaymentReturn([FromQuery] Dictionary<string, string> query)
        {
            var queryCollection = new QueryCollection(query.ToDictionary(kvp => kvp.Key, kvp => new Microsoft.Extensions.Primitives.StringValues(kvp.Value)));
            var paymentResponse = _vnPayService.PaymentExecute(queryCollection);

            if (!paymentResponse.Success)
            {
                _logger.LogError("Payment verification failed. Response: {Response}", paymentResponse);
                return BadRequest("Payment verification failed.");
            }

            try
            {
                var order = await GetInvoiceFromCacheAsync();

                var invoiceCreate = new Order
                {
                    DateTime = DateTime.Now,
                    CustomerID = order.CustomerID,
                    TotalPrice = order.TotalPrice,
                    Status = OrderStatus.Pending,
                    TransactionID = paymentResponse.TransactionId,
                };

                var createdInvoice = await _orderRepo.AddAsync(invoiceCreate);
                _logger.LogInformation("Invoice created successfully. Invoice ID: {InvoiceId}", createdInvoice.OrderID);

                
                var cartItems = await _cartItemRepo.GetCardItemByCustomerId(createdInvoice.CustomerID);
                if (cartItems == null || !cartItems.Any())
                {
                    _logger.LogError("No cart items found for cart ID: {CartID}", createdInvoice.CustomerID);
                    return BadRequest("No cart items found.");
                }

                var orderItems = cartItems.Select(cartItem => new OrderItem
                {
                    Quantity = cartItem.Quantity,
                    ProductSizeID = cartItem.ProductSize.ProductSizeID,
                    OrderID = createdInvoice.OrderID,
                }).ToList();

                foreach (var orderItem in orderItems)
                {
                    await _orderItemRepo.AddAsync(orderItem);
                }
                _logger.LogInformation("Order items added successfully!");

                // Xóa giỏ hàng sau khi tạo đơn hàng thành công
                await _cartItemRepo.ClearCartAsync(invoiceCreate.CustomerID);

                var redirectUrl = $"{_config["VnPay:VnPayFrontendReturnUrl"]}?success={paymentResponse.Success}&orderId={paymentResponse.OrderId}&transactionId={paymentResponse.TransactionId}";
                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the payment return. Response: {Response}", paymentResponse);
                return StatusCode(500, "An error occurred while processing the payment return.");
            }
        }

        private async Task<OrderCreateDto> GetInvoiceFromCacheAsync()
        {
            var invoiceJson = await _distributedCache.GetStringAsync("Invoice");
            return string.IsNullOrEmpty(invoiceJson) ? new OrderCreateDto() : JsonSerializer.Deserialize<OrderCreateDto>(invoiceJson);
        }

        private async Task SetInvoiceToCacheAsync(OrderCreateDto invoice)
        {
            var invoiceJson = JsonSerializer.Serialize(invoice);
            await _distributedCache.SetStringAsync("Invoice", invoiceJson);
        }
    }
}
