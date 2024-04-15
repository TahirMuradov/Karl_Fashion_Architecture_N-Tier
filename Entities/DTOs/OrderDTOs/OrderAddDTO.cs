using Entities.Concrete;
using Entities.DTOs.CheckOutDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.OrderDTOs
{
    public class OrderAddDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string? OrderPDfUrl { get; set; }
        public string? OrderNumber { get; set; }

        public string? Message { get; set; }

        public List<OrderProduct>? OrderProducts { get; set; }

        public string PaymentMethodId { get; set; }
        public string ShippingMethodId { get; set; }
    }
}
