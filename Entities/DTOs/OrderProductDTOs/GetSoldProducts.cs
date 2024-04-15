namespace Entities.DTOs.OrderProductDTOs
{
    public class GetSoldProducts
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public DateTime SoldDate { get; set; }
   public string OrderNumber { get; set; }
        public string Size { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
    }
}
