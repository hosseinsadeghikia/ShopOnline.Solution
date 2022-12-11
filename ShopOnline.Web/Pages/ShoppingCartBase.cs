using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.DTOs;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService CartItemsLocalStorageService { get; set; }

        public List<CartItemDto>? ShoppingCartItems { get; set; }
        public string ErrorMessage { get; set; }

        protected string? TotalPrice { get; set; }

        public int TotalQuantity { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                //ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                ShoppingCartItems = await CartItemsLocalStorageService.GetCollection();
                CartChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task DeleteCartItem_Click(int id)
        {
            await ShoppingCartService.DeleteItem(id);
            await RemoveCartItem(id);
            CartChanged();
        }

        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {
            try
            {
                if (qty > 0)
                {
                    var updateDtoItem = new CartItemQtyUpdateDto
                    {
                        CartItemId = id,
                        Qty = qty
                    };

                    var returnedUpdateItemDto = await ShoppingCartService.UpdateQty(updateDtoItem);

                    if (returnedUpdateItemDto != null)
                    {
                        await UpdateItemTotalPrice(returnedUpdateItemDto);
                        CartChanged();
                        await MakeUpdateQtyButtonVisible(id, false);
                    }
                }
                else
                {
                    var item = ShoppingCartItems?.FirstOrDefault(x => x.Id == id);

                    if (item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task UpdateQty_Input(int id)
        {
            await MakeUpdateQtyButtonVisible(id, true);
        }

        private async Task MakeUpdateQtyButtonVisible(int id, bool visible)
        {
            await JsRuntime.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible); //input is js function name
        }

        private async Task UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);

            if (item != null)
            {
                item.TotalPrice = item.Price * item.Qty;
            }

            await CartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
        }

        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems?.Sum(x => x.TotalPrice).ToString("C");
        }

        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Sum(x => x.Qty);
        }

        private CartItemDto? GetCartItem(int id)
        {
            return ShoppingCartItems?.FirstOrDefault(x => x.Id == id)!;
        }

        private async Task RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItem(id);

            if (cartItemDto != null)
                ShoppingCartItems?.Remove(cartItemDto);

            await CartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
        }

        private void CartChanged()
        {
            CalculateCartSummaryTotals();
            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
        }
    }
}
