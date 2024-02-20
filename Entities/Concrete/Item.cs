using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Item
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public int count { get; set; }
        public decimal Price { get; set; }
        public DateTime DateOfSale { get; set; }
        public List<CartItem> Carts { get; set; }
    }
}
