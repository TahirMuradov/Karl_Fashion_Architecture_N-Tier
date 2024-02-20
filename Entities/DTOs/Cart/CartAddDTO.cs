using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Cart
{
    public class CartAddDTO
    {
        public Guid ProductId { get; set; }
       
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
    
        public string Size { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Guid ShippingMethodId { get; set; }


    }
}
