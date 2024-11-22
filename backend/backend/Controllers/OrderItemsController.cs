using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Entities;
using backend.Repositories.EntitiesRepo;
using AutoMapper;
using backend.Dtos;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IRepo<OrderItem> _orderItemRepo;
        private readonly IMapper _mapper;

        public OrderItemsController(IRepo<OrderItem> orderItemRepo, IMapper mapper)
        {
            _orderItemRepo = orderItemRepo;
            _mapper = mapper;
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemReadDto>>> GetOrderItems()
        {
            var orderItems = await _orderItemRepo.GetAllAsync();
            var orderItemsDto = _mapper.Map<IEnumerable<OrderItemReadDto>>(orderItems);
            return Ok(orderItemsDto);
        }

        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemReadDto>> GetOrderItem(int id)
        {
            var orderItem = await _orderItemRepo.GetByIdAsync(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            var orderItemDto = _mapper.Map<OrderItemReadDto>(orderItem);
            return Ok(orderItemDto);
        }

        // PUT: api/OrderItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, OrderUpdateDto orderItem)
        {
            var existingOrderItem = await _orderItemRepo.GetByIdAsync(id);
            if (existingOrderItem == null)
            {
                return NotFound();
            }

            var orderItemEntity = _mapper.Map<OrderItem>(orderItem);
            bool updated = await _orderItemRepo.UpdateAsync(orderItemEntity);
            if (!updated)
            {
                return BadRequest("Unable to update the order item.");
            }

            return NoContent();
        }

        // POST: api/OrderItems
        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItemCreateDto orderItem)
        {
            var orderItemEntity = _mapper.Map<OrderItem>(orderItem);
            var added = await _orderItemRepo.AddAsync(orderItemEntity);
            if (added == null)
            {
                return BadRequest("Unable to add the order item.");
            }

            return CreatedAtAction("GetOrderItem", new { id = added.OrderItemID }, added);
        }

        // DELETE: api/OrderItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _orderItemRepo.GetByIdAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            bool deleted = await _orderItemRepo.DeleteAsync(id);
            if (!deleted)
            {
                return BadRequest("Unable to delete the order item.");
            }

            return NoContent();
        }

        
    }
}
