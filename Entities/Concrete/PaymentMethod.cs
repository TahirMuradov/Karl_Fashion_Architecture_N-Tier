using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class PaymentMethod:IEntity
    {
        public  Guid Id { get; set; }
        public bool Api { get; set; }
        public List<PaymentMethodLaunge>PaymentMethodLaunguages { get; set; }
    }
}
