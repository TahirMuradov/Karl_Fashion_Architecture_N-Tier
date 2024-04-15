using Entities.Concrete.EnumClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.OrderDTOs
{
    public class OrderGetDTO
    {
       public string OrderId { get; set; }
        public string OrderNubmer {  get; set; }
        public OrderStatus Status { get; set; }
        public string OrderPdfUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string? Message { get; set; }
        public string PaymentMethodId { get; set; }
        public string ShippingMethodId { get; set; }
    }
}
