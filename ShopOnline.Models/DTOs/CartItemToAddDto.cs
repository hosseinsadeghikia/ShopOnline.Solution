namespace ShopOnline.Models.DTOs
{
    public class CartItemToAddDto
    {
        public int CardId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
    }
}
