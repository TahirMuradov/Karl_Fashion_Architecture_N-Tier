using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Cart
{
    public class CartAddItemCookieDTO
    {
        public string Id { get; set; }
        public string  PhotoUrl { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string Size {  get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
     
     
    }
}
