using Microsoft.AspNetCore.Components;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services;
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

        private async Task ClearLocalStorage()
        {
            await ProductsLocalStorageService.RemoveCollection();
            await CartItemsLocalStorageService.RemoveCollection();
        }
    }
}
