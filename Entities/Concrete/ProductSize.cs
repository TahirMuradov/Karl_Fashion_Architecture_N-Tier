using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ProductSize:BaseEntity,IEntity
    {
       public Guid ProductId { get; set; }
        public Product Products { get; set;}
        public Guid SizeId { get; set; }
        public Size Sizes {  get; set;}
    }
}
