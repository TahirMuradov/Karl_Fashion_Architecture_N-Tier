using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ShippingMethods
{
    public class AddShippingMethodDTO
    {
        public List<string> LangCode { get; set; }
        public List<string> ShippingContent { get; set; }

        public decimal DeliveryPrice { get; set; }

    }
}
