using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class CheckoutBase : ComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected IEnumerable<CartItemDto>? ShoppingCartItems { get; set; }

        protected int TotalQty { get; set; }

        protected string PaymentDescription { get; set; }

        protected decimal PaymentAmount { get; set; }

        [Inject]
        protected IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        protected IManageCartItemsLocalStorageService CartItemsLocalStorageService { get; set; }

        protected string DisplayButtons { get; set; } = "block";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                //ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                ShoppingCartItems = await CartItemsLocalStorageService.GetCollection();

                if (ShoppingCartItems != null && ShoppingCartItems.Any())
                {
                    Guid orderGuid = Guid.NewGuid();

                    PaymentAmount = ShoppingCartItems.Sum(x => x.TotalPrice);
                    TotalQty = ShoppingCartItems.Sum(x => x.Qty);
                    PaymentDescription = $"O_{HardCoded.UserId}_{orderGuid}";
                }
                else
                {
                    DisplayButtons = "none";
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    await JSRuntime.InvokeVoidAsync("initPayPalButton"); // js function name in index.html
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
