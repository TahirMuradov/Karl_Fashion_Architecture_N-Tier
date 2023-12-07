using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class UserActions
    {
        public Guid Id { get; set; }
        public string ActionName { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }             
        public Guid? CategoryId { get; set; }  
        public Category? Category { get; set; }
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public User User  { get; set; }
    
       
    }
}
