using Core.Entites;
using Entities.Concrete;

namespace Entities
{
    public class Product: BaseEntity, IEntity
    {
 
        public string ProductName { get; set; }
        public string ProductInformation { get; set; }
        public decimal DisCount { get; set; }
        public decimal Price { get; set; }
        public bool isFeatured { get; set; }
        public Guid CategoryId {  get; set; } 
        public Category Category { get; set; }
             
     public List<ProductSize> Sizes { get; set; }
        public List<string> Color {  get; set; }
        public List<Picture> Pictures { get; set;}
   public List<ProductLanguages> productLanguages { get; set; }

    }
}
