using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class ProductGetAdminListDTO
    {
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public List<string> ImgUrls { get; set; }
        public Dictionary<int,int> ProductSizeAndCount {  get; set; }
        public decimal ProductPrice { get; set; }
        public decimal DisCount { get; set; }
        public  List<string> categoryName { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ProductGetAdminListDTO))
                return false;

            ProductGetAdminListDTO other = (ProductGetAdminListDTO)obj;
            return this.ProductId == other.ProductId;
        }

        public override int GetHashCode()
        {
            return ProductId.GetHashCode();
        }
    }
  
}
