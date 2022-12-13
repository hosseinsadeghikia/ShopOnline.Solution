using Microsoft.AspNetCore.Components;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsByCategoryBase : ComponentBase
    {
        [Parameter]
        public int CategoryId { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        public IEnumerable<ProductDto>? Products { get; set; }

        public string? CategoryName { get; set; }

        public string ErrorMessage { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                Products = await GetProductCollectionByCategoryId(CategoryId);

                if (Products != null && Products.Any())
                {
                    var productDto = Products.FirstOrDefault(x => x.CategoryId == CategoryId);

                    if (productDto != null)
                    {
                        CategoryName = productDto.CategoryName;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private async Task<IEnumerable<ProductDto>?> GetProductCollectionByCategoryId(int categoryId)
        {
            var productCollection = await ProductService.GetItems();

            if (productCollection != null)
            {
                return productCollection.Where(p => p.CategoryId == categoryId);
            }

            return await ProductService.GetItemsByCategory(categoryId);
        }
    }
}
