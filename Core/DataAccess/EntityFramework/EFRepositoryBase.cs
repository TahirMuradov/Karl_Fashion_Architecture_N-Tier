using Core.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework
{
    public class EFRepositoryBase<TEntity, TContext> : IRepositoryBase<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext, new()
    {
        public void AddRepos(TEntity entity)
        {
            using var context = new TContext();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Added;

            context.SaveChanges();
        }

        public async void AddAsyncRepos(TEntity entity)
        {
            using var context = new TContext();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Added;

          await  context.SaveChangesAsync();
        }

        public void DeleteRepos(TEntity entity)
        {
            using var context = new TContext();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Deleted;

            context.SaveChanges();
        }

        public async void DeleteAsyncRepos(TEntity entity)
        {
            using var context = new TContext();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Added;

          await  context.SaveChangesAsync();
        }

      

        public List<TEntity> GetAllRepos(Expression<Func<TEntity, bool>> expression)
        {
             using var context = new TContext();
            return expression == null
                 ? context.Set<TEntity>().ToList() :
                 context.Set<TEntity>().Where(expression).ToList();
        }

        public void UpdateRepos(TEntity entity)
        {
            using var context = new TContext();
            var updateEntity = context.Entry(entity);
            updateEntity.State = EntityState.Modified;
            context.SaveChanges();
        }

        public async void UpdateAsyncRepos(TEntity entity)
        {
            using var context = new TContext();
            var updateEntity = context.Entry(entity);
            updateEntity.State = EntityState.Modified;
        await    context.SaveChangesAsync();
        }

        public async Task<TEntity> GetAsyncRepos(Expression<Func<TEntity, bool>> expression)
        {
            using var context = new TContext();
            return
                await context.Set<TEntity>().SingleOrDefaultAsync(expression);
        }

        public TEntity GetRepos(Expression<Func<TEntity, bool>> expression)
        {
         using var context= new TContext();
            return context.Set<TEntity>().SingleOrDefault(expression);
        }

        public Task<List<TEntity>> GetAllAsyncRepos(Expression<Func<TEntity, bool>> expression)
        {
          using var context=new TContext();
            return expression==null ?
                context.Set<TEntity>().ToListAsync()
                :context.Set<TEntity>().Where(expression).ToListAsync();

        }
    }
}
