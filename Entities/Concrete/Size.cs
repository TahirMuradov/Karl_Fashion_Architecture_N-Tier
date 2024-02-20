using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Size:BaseEntity,IEntity
    {
      
        public int NumberSize { get; set; }
    

        public List<ProductSize> ProductSize { get; set; }

    }
}
