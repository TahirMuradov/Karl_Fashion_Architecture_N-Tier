using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Cart
{
    public class ProductModalDTO
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public Dictionary<int,int> Product_Size { get; set; }
        public List<string> Product_Category { get; set; }
        public string PicturesUrl { get; set; }
        public decimal Price { get; set; }
        public decimal DisCount { get; set; }
        public string Color { get; set; }

    }
}
