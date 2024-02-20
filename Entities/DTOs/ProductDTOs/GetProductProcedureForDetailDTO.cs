using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class GetProductProcedureForDetailDTO
    {
        public Guid ProductId { get; set; }
        public string  Color { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal DisCount { get; set; }
        public bool IsFeatured {  get; set; }
        public List<string> PicturesUrls { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; } 
        public string SeoUrl { get; set; }
        public int SizeStockCount { get; set; }
        public int NumberSize {  get; set; }

    }
}
