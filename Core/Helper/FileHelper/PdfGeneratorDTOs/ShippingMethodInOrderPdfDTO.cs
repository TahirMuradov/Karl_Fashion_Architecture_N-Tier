using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.CheckOutDTOs
{
    public class ShippingMethodInOrderPdfDTO
    {
        public string Id { get; set; }
        public string ShippingContent { get; set; }
        public decimal Price { get; set; }
    }
}
