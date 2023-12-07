using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class CategoryLanguage:IEntity
    {
      public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string LangCode { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
     
    }
}
