using Core.Entites;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Picture:BaseEntity,IEntity

    {
       
        public string PictureUrl { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }

    }
}
