using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class FilterProductDTO
    {
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Category { get; set; }
    }
}
