using Core.DataAccess.EntityFramework;
using DataAccess.Concrete.SQLserver;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EFProductDAL:EFRepositoryBase<Product,AppDbContext>
    {

    }
}
