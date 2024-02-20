using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Cart:IEntity
    {
        public Guid Id { get; set; }
        public List<CartItem> Items { get; set; }
        public Guid ShippingMethodsId { get; set; }
        public ShippingMethods ShippingMethods { get; set; }

       public string? UserId { get; set; }
        public User? User { get; set; }
        public string? FirstName { get; set; }
        public string ?LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
