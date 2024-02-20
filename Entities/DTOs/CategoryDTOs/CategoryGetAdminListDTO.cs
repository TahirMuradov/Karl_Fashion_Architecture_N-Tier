using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.CategoryDTOs
{
    public class CategoryGetAdminListDTO
    {
        public string id {  get; set; }
        public List<string> CategoryName { get; set; }
        public List<string> LaunguageCode { get; set; }
        public string UserName { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set;}
        public string ? UpdatedUserId { get; set; }
    }
}
