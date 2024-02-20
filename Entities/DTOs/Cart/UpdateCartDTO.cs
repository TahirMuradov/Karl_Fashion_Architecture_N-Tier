using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Cart
{
    public class UpdateCartDTO
    {
        public string Id { get; set; }
        public string Quantity { get; set; }
        public string Size { get; set; }
    }
}
