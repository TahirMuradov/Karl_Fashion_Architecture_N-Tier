using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class GetProductProcedureAdminDTO
    {
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        public bool isFeatured { get; set; }
        public List<string> PicturesUrls {  get; set; }
        public Guid ProductLanguageId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string SeoUrl { get; set; }

        public Guid CategoryProductId { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Guid CategoryLanguageId { get; set; }
        public string CategoryLanguageCode { get; set; }

        public Guid ProductSizeId { get; set; }
        public Guid SizeId { get; set; }
        public int NumberSize { get; set; }
        public int SizeStockCount { get; set; }
    }

}
