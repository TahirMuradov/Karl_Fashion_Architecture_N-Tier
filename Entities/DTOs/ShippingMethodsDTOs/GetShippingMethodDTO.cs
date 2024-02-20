using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ShippingMethodsDTOs
{
    public class GetShippingMethodDTO
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public decimal Price { get; set; }
    }
}
