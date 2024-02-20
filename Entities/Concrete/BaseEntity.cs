using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }    
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime? UpdatedDate { get; set;}
        public string? UpdatedUserId { get; set; }
      
        
    }
}
