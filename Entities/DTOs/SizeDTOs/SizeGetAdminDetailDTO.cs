using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.SizeDTOs
{
    public class SizeGetAdminDetailDTO
    {
        public string SizeId { get; set; }
        public int Size { get; set; }
        public List< string> ProductName { get; set; }
        public int StockSizeCount { get; set; }
        public string CreatorUserName { get; set; }
    }
}
