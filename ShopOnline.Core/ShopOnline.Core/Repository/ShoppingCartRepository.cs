using Microsoft.EntityFrameworkCore;
using ShopOnline.Core.IRepository;
using ShopOnline.Data;
using ShopOnline.Models.DTOs;

namespace ShopOnline.Core.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await _context.CartItems.
                AnyAsync(x => x.CartId == cartId &&
                                      x.ProductId == productId);
        }

        public async Task<CartItem?> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (await CartItemExists(cartItemToAddDto.CardId, cartItemToAddDto.ProductId) == false)
            {
                var item = await (from product in _context.Products
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new CartItem
                                  {
                                      CartId = cartItemToAddDto.CardId,
                                      ProductId = product.Id,
                                      Qty = cartItemToAddDto.Qty
                                  }).SingleOrDefaultAsync();

                if (item != null)
                {
                    var result = await _context.CartItems.AddAsync(item);
                    await _context.SaveChangesAsync();
                    return result.Entity;
                }
            }

            return null;
        }

        public async Task<CartItem?> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await _context.CartItems.FindAsync(id);

            if (item != null)
            {
                var product = await _context.Products.Where(x => x.Id == item.ProductId)
                    .FirstOrDefaultAsync();

                if (product != null)
                {
                    if (product.Qty >= cartItemQtyUpdateDto.Qty)
                    {
                        item.Qty = cartItemQtyUpdateDto.Qty;
                        await _context.SaveChangesAsync();
                    }

                    return item;
                }
            }

            return null;
        }

        public async Task<CartItem?> DeleteItem(int id)
        {
            var item = await _context.CartItems.FindAsync(id);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            return item;
        }

        public async Task<CartItem?> GetItem(int id)
        {
            return await (from cart in _context.Carts
                          join cartItem in _context.CartItems
                              on cart.Id equals cartItem.CartId
                          where cartItem.Id == id
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.Id
                          }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            var res = await (from cart in _context.Carts
                             join cartItem in _context.CartItems
                                 on cart.Id equals cartItem.CartId
                             where cart.UserId == userId
                             select new CartItem
                             {
                                 Id = cartItem.Id,
                                 ProductId = cartItem.ProductId,
                                 Qty = cartItem.Qty,
                                 CartId = cartItem.CartId
                             }).ToListAsync();

            return res;
        }
    }
}
