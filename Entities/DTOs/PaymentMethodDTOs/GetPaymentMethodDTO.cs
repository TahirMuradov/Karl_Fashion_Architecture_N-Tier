using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.PaymentMethodDTOs
{
    public class GetPaymentMethodDTO
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public bool Api { get; set; }
    }
}
