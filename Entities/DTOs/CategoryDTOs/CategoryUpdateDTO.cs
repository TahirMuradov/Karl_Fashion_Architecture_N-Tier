using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.CategoryDTOs
{
    public class CategoryUpdateDTO
    {
        public Guid CategoryId { get; set; }
        public Guid LanguagesId { get; set; }
        public List<string> LanguageCode { get; set; }
        public List< string> CategoryNewName { get; set; }
    }
}
