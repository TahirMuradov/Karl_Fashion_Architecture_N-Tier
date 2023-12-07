using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class ProductAddDTO
    {

      public IDictionary<int,int> SizesAndStockCount { get; set; }
        public bool isFeatured {  get; set; }
        public decimal Price { get; set; }
        public decimal DisCount { get; set; }
        public Guid CategoryId { get; set; }
        public string UserId { get; set; }
        public List<string> Color {  get; set; }

        public List<string> ProductName;
        public List<string> ProductInformation;
        public List<string> LangCode { get; set; }

        public List<string> SeoUrls { get; set; }
        public List<string> PhotoUrls { get; set; }

    }
}
