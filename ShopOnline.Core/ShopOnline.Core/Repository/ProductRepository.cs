using Microsoft.EntityFrameworkCore;
using ShopOnline.Core.IRepository;
using ShopOnline.Data;

namespace ShopOnline.Core.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _shopOnlineDbContext;

        public ProductRepository(ApplicationDbContext shopOnlineDbContext)
        {
            _shopOnlineDbContext = shopOnlineDbContext;
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            //var products = await _shopOnlineDbContext.Products.ToListAsync();
            var products = await _shopOnlineDbContext.Products
                                                                 .Include(x => x.ProductCategory).ToListAsync();
            return products;
        }

        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await _shopOnlineDbContext.ProductCategories.ToListAsync();
            return categories;
        }

        public async Task<Product?> GetItem(int id)
        {
            //var product = await _shopOnlineDbContext.Products.FindAsync(id);
            var product = await _shopOnlineDbContext.Products
                                                    .Include(x => x.ProductCategory)
                                                    .SingleOrDefaultAsync(x => x.Id == id);
            return product;
        }

        public async Task<ProductCategory?> GetCategory(int id)
        {
            var category = await _shopOnlineDbContext.ProductCategories.SingleOrDefaultAsync(x => x.Id == id);
            return category;
        }

        public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
        {
            //var products = await (from product in _shopOnlineDbContext.Products
            //where product.CategoryId == id
            //select product).ToListAsync();

            var products = await _shopOnlineDbContext.Products
                                                                 .Include(x => x.ProductCategory)
                                                                 .Where(x => x.CategoryId == id).ToListAsync();

            return products;
        }
    }
}
