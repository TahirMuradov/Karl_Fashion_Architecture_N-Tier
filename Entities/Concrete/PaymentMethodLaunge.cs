using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class PaymentMethodLaunge:IEntity
    {

        public Guid Id { get; set; }
        public string LangCode { get; set; }
        public string Content { get; set; }
        public Guid PaymentMehtodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
