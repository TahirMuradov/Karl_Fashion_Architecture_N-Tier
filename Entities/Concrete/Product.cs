using Core.Entites;
using Entities.Concrete;

namespace Entities
{
    public class Product : BaseEntity, IEntity
    {

        public string ProductCode { get; set; }

        public decimal DisCount { get; set; }
        public decimal Price { get; set; }
        public bool isFeatured { get; set; }

        public List<CategoryProduct> ProductCategories { get; set; }
        public List<OrderProduct>? OrderProducts { get; set; }

        public List<ProductSize> ProductSizes { get; set; }
        public string Color { get; set; }
        public List<string> PicturesUrls { get; set; }
        public List<ProductLanguages> productLanguages { get; set; }

    }
}
