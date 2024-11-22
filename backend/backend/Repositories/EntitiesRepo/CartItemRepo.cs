using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.EntitiesRepo
{
    public class CartItemRepo : GenericRepo<CartItem>
    {
        public CartItemRepo(EcommerceDBContext context) : base(context)
        {

        }

        public async Task<IEnumerable<CartItem>> GetCardItemByCustomerId(int id)
        {
            return await _context.CartItems
                          .Where(ci => ci.CustomerID == id)
                          .Include(ci => ci.ProductSize) 
                          .Include(ci => ci.ProductSize.Product)
                          .ToListAsync();
        }
        public async Task ClearCartAsync(int customerId)
        {
            // Lấy tất cả các CartItems của giỏ hàng theo CartID
            var cartItems = await _context.CartItems.Where(ci => ci.CustomerID == customerId).ToListAsync();

            if (cartItems.Any())
            {
                // Xóa tất cả các CartItems
                _context.CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CartItem> GetCartItemByCustomerIdAndProductIdAsync(int customerId, int productSizeId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CustomerID == customerId && ci.ProductSizeID == productSizeId);
        }

    }
}
