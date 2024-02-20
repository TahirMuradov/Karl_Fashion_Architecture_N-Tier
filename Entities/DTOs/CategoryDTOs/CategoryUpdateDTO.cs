using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.CategoryDTOs
{
    public class CategoryUpdateDTO
    {
        public string CategoryId { get; set; }
        public List<Category>? SubCategoryName { get; set; }
        public string UpdatedUserId { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public List<string> NewCategoryName { get; set; }
        public List<string> LangCode { get; set; }
    }
}
