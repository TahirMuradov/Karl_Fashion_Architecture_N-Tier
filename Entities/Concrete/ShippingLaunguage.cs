using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ShippingLaunguage: IEntity
    {
        public Guid Id { get; set; }
        public string LangCode { get; set; }
        public string Content { get; set; }
        public Guid ShippingMethodId { get; set; }
        public ShippingMethods ShippingMethod { get; set; }
    }
}
