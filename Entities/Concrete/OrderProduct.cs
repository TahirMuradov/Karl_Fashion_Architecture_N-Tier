using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class OrderProduct:IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Size { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
    }
}
