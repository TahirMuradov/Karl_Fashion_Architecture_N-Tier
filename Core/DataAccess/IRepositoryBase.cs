using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    public interface IRepositoryBase<TEntity>
        where TEntity : class,IEntity
    {
        Task<bool> AddAsync(TEntity entity);
        bool Add(TEntity entity);
       Task <bool>UpdateAsync(TEntity entity);
        bool Update(TEntity entity);
       Task<bool> DeleteAsync(TEntity entity);
        bool Delete(TEntity entity);

       Task< TEntity> GetAsync(Expression<Func<TEntity, bool>> expression);
        TEntity Get(Expression<Func<TEntity, bool>> expression);
      Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression);
       List<TEntity> GetAll(Expression<Func<TEntity, bool>> expression);
        

    }
}
