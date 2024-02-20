using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class ProductUpdateDTO
    {
        public Guid Id { get; set; }
        public List<int> ProductSizes { get; set; }
        public List<int> SizesCount { get; set; }
        public string ProductCode { get; set; }
        public bool isFeatured { get; set; }
        public decimal Price { get; set; }
        public decimal DisCount { get; set; }
        public List<Guid> CategoryId { get; set; }
        public string UpdatedUserId { get; set; }
        public string Color { get; set; }

        public List<string> ProductName { get; set; }
        public List<string> ProductDescrption { get; set; }
        public List<string> LangCode { get; set; }

        public List<string> SeoUrls { get; set; }
        public List<string> PicturesUrls { get; set; }
    }
}
