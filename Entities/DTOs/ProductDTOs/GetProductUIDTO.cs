using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class GetProductUIDTO
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }

        public Dictionary<int,int> Product_Size { get; set; }
        public  List<string> Product_Category { get; set; }
        public List<string> PicturesUrls { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Price { get; set; }
        public decimal DisCount { get; set; }
       
        public string Color { get; set; }   


    }
}
