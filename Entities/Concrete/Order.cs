using Core.Entites;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Order:BaseEntity,IEntity
    {

        public string OrderNumber { get; set; }
        public string Location { get; set; }
        public List<Product> Product { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public string? Message { get; set; }
        public string UserId { get; set; }
        public User  User { get; set; }
    }
}
