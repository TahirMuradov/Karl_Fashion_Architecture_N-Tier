using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLserver;
using Entities;
using Entities.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EFCtegoryLaunguageDAL : EFRepositoryBase<CategoryLanguage, AppDbContext>, ICategoryLaunguageDAL
    {
     
    }
}
