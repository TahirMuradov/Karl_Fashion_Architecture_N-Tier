using Core.Entites;
using Entities.Concrete;
using Entities.Concrete.EnumClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Order:BaseEntity,IEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string  PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string OrderNumber { get; set; }
        public string? Message { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<OrderProduct> Products { get; set; }

 
    }
}
