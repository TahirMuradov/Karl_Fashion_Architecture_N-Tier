using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.UserDTOs
{
    public class UserAddRoleDTO
    {
        public string UserId { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
