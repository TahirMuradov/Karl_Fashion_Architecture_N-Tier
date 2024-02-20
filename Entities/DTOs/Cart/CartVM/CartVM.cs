namespace Entities.DTOs.Cart.CartVM
{
    public class CartVM
    {
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public List<CartAddItemCookieDTO> CartItems { get; set; }
    }
}
