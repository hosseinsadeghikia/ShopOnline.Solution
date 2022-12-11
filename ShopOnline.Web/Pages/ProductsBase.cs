using Microsoft.AspNetCore.Components;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        IManageProductsLocalStorageService ProductsLocalStorageService { get; set; }

        [Inject]
        IManageCartItemsLocalStorageService CartItemsLocalStorageService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public IEnumerable<ProductDto>? Products { get; set; }

        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await ClearLocalStorage();

                //Products = await ProductService.GetItems();
                Products = await ProductsLocalStorageService.GetCollection();

                //var shoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                var shoppingCartItems = await CartItemsLocalStorageService.GetCollection();

                var totalQty = shoppingCartItems.Sum(x => x.Qty);

                ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in Products
                group product by product.CategoryId into prodByCatGroup
                orderby prodByCatGroup.Key
                select prodByCatGroup;
        }

        protected string? GetCategoryName(IGrouping<int, ProductDto> groupedProductDto)
        {
            return groupedProductDto.FirstOrDefault(pg => pg.CategoryId == groupedProductDto.Key)?.CategoryName;
        }

        private async Task ClearLocalStorage()
        {
            await ProductsLocalStorageService.RemoveCollection();
            await CartItemsLocalStorageService.RemoveCollection();
        }
    }
}
