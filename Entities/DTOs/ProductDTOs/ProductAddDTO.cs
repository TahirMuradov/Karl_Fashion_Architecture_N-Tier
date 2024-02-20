using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class ProductAddDTO
    {

      public List<string> Sizes { get; set; }
        public List<int>SizesCount { get; set; }
        public string ProductCode { get; set; }
        public bool isFeatured {  get; set; }
        public decimal Price { get; set; }
        public decimal DisCount { get; set; }
        public List<Guid> CategoryId { get; set; }
        public string UserId { get; set; }
        public string Color {  get; set; }

        public List<string> ProductName { get; set; }
        public List<string> ProductDescrption { get; set; }
        public List<string> LangCode { get; set; }

        public List<string> SeoUrls { get; set; }
        public List<string> PhotoUrl { get; set; }

    }
}
