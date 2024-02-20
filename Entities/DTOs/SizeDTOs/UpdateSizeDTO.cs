using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.SizeDTOs
{
    public class UpdateSizeDTO
    {
        public string SizeId { get; set; }
        public int NewSizeContent { get; set; }
        public string UpdatedUserId { get; set; }
    }
}
