using Core.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ProductSize:IEntity
    {
     
        public Guid Id { get; set; }
      
       public Guid ProductId { get; set; }
        public Product Product { get; set;}
   
        public Guid SizeId { get; set; }
        public Size Size {  get; set;}
        public int SizeStockCount { get; set; }
    }
}
