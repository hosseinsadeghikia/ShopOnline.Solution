using ShopOnline.Core.DTOs;
using ShopOnline.Models.DTOs;

namespace ShopOnline.Web.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>?> GetItems();
        Task<ProductDto?> GetItem(int id);

        Task<IEnumerable<ProductCategoryDto>?> GetProductCategories();
        Task<IEnumerable<ProductDto>?> GetItemsByCategory(int categoryId);
    }
}
