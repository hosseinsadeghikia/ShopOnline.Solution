using Microsoft.AspNetCore.Components;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductDetailsBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public ProductDto? Product { get; set; }

        public string ErrorMessage { get; set; }

        private List<CartItemDto>? ShoppingCartItems { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                Product = await GetProductById(Id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var cartItemDto = await ShoppingCartService.AddItem(cartItemToAddDto);

                if (cartItemDto != null)
                {
                    ShoppingCartItems?.Add(cartItemDto);
                }

                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private async Task<ProductDto?> GetProductById(int id)
        {
            var productDto = await ProductService.GetItems();

            if (productDto != null)
            {
                return productDto.SingleOrDefault(x => x.Id == id);
            }

            return null;
        }
    }
}
