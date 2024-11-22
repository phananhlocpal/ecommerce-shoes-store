using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Repositories.EntitiesRepo;
using backend.Dtos;  // Assuming DTOs are in this namespace
using AutoMapper;
using backend.Entities;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly CartItemRepo _cartItemRepo;
        private readonly IMapper _mapper;

        public CartItemsController(CartItemRepo cartItemRepo, IMapper mapper)
        {
            _cartItemRepo = cartItemRepo;
            _mapper = mapper;
        }

        // GET: api/CartItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemReadDto>>> GetCartItems()
        {
            var cartItems = await _cartItemRepo.GetAllAsync();
            var cartItemsDto = _mapper.Map<IEnumerable<CartItemReadDto>>(cartItems);
            return Ok(cartItemsDto);
        }

        // GET: api/CartItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CartItemReadDto>>> GetCartItem(int id)
        {
            var cartItems = await _cartItemRepo.GetCardItemByCustomerId(id);

            if (cartItems == null || !cartItems.Any())
            {
                return Ok(null);
            }

            var cartItemsDto = _mapper.Map<IEnumerable<CartItemReadDto>>(cartItems);
            return Ok(cartItemsDto);
        }

        // PUT: api/CartItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItem(int id, CartItemUpdateDto cartItem)
        {
            // Check if the cart item exists in the repository before updating
            var existingCartItem = await _cartItemRepo.GetByIdAsync(id);
            if (existingCartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            // Map the DTO to the entity
            _mapper.Map(cartItem, existingCartItem);

            // Assuming UpdateAsync will modify the item in the database and return the updated entity
            var updatedCartItem = await _cartItemRepo.UpdateAsync(existingCartItem);
            if (updatedCartItem == null)
            {
                return BadRequest("Failed to update the cart item.");
            }

            // Return NoContent when update is successful
            return NoContent();
        }

        // POST: api/CartItems
        [HttpPost]
        public async Task<ActionResult<CartItemReadDto>> PostCartItem(CartItemCreateDto cartItem)
        {
            if (cartItem == null)
            {
                return BadRequest("Cart item is required.");
            }

            // Map DTO to the entity
            var newCartItem = _mapper.Map<CartItem>(cartItem);

            var result = await _cartItemRepo.AddAsync(newCartItem);
            if (result == null)
            {
                return BadRequest("Unable to add the cart item.");
            }

            var cartItemDto = _mapper.Map<CartItemReadDto>(result);
            return CreatedAtAction("GetCartItem", new { id = cartItemDto.CartItemID }, cartItemDto);
        }

        // DELETE: api/CartItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var result = await _cartItemRepo.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/CartItems/removeCart/5
        [HttpDelete("removeCart/{customerId}")]
        public async Task<IActionResult> RemoveCartByCustomerId(int customerId)
        {
            var cartItems = await _cartItemRepo.GetCardItemByCustomerId(customerId);
            foreach (var cartItem in cartItems)
            {
                await _cartItemRepo.DeleteAsync(cartItem.CartItemID);
            }
            return NoContent();
        }

        [HttpPost("updateSession")]
        public async Task<IActionResult> UpdateCartItemFromSession(List<CartItemCreateDto> cartItemsFromSession)
        {
            if (cartItemsFromSession == null || !cartItemsFromSession.Any())
            {
                return BadRequest("No cart items provided.");
            }

            foreach (var sessionItem in cartItemsFromSession)
            {
                // Kiểm tra xem cart item có tồn tại dựa trên CartID và ProductID
                var existingCartItem = await _cartItemRepo.GetCartItemByCustomerIdAndProductIdAsync(sessionItem.CustomerID, sessionItem.ProductSizeID);

                if (existingCartItem == null)
                {
                    // Nếu cart item không tồn tại, thêm mới
                    var newCartItem = _mapper.Map<CartItem>(sessionItem);
                    await _cartItemRepo.AddAsync(newCartItem);
                }
                else
                {
                    // Nếu cart item đã tồn tại, cập nhật số lượng và các thuộc tính cần thiết
                    existingCartItem.Quantity += sessionItem.Quantity;

                    // Cập nhật trong cơ sở dữ liệu
                    await _cartItemRepo.UpdateAsync(existingCartItem);
                }
            }

            return NoContent();
        }

    }
}
