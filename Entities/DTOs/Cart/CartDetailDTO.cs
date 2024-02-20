using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Cart
{
    public class CartDetailDTO
    {
        public string Id { get; set; }
        public string   ProductName { get; set; }
        public string PictureUrl { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int MaxQuantity { get; set; }
        public int NeededQuantity { get; set; }
       

    }
}
