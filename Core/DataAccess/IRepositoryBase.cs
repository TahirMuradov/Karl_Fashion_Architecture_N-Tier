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
        void AddAsyncRepos(TEntity entity);
        void AddRepos(TEntity entity);
        void UpdateAsyncRepos(TEntity entity);
        void UpdateRepos(TEntity entity);
        void DeleteAsyncRepos(TEntity entity);
        void DeleteRepos(TEntity entity);

       Task< TEntity> GetAsyncRepos(Expression<Func<TEntity, bool>> expression);
        TEntity GetRepos(Expression<Func<TEntity, bool>> expression);
      Task<  List<TEntity>> GetAllAsyncRepos(Expression<Func<TEntity, bool>> expression);
       List<TEntity> GetAllRepos(Expression<Func<TEntity, bool>> expression);

    }
}
