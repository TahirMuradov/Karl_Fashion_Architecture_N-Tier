using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ShippingMethods: IEntity
    {
        public Guid Id { get; set; }
      public List<ShippingLaunguage> ShippingLaunguages { get; set; }
        public decimal DeliveryPrice { get; set; }
    }
}
