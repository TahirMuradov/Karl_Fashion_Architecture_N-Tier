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
        public bool Add(TEntity entity)
        {
            using var context = new TContext();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Added;

            context.SaveChanges();
            return true;
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            using var context = new TContext();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Added;

          await  context.SaveChangesAsync();
            return true;
        }

        public bool Delete(TEntity entity)
        {
            using var context = new TContext();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Deleted;

            context.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            using var context = new TContext();
            var addEntity = context.Entry(entity);
            addEntity.State = EntityState.Added;

        var result=  await  context.SaveChangesAsync();
        
            return true;
        }

       public List<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
             using var context = new TContext();
            return expression == null
                 ? context.Set<TEntity>().ToList() :
                 context.Set<TEntity>().Where(expression).ToList();
        }

        public bool Update(TEntity entity)
        {
            using var context = new TContext();
            var updateEntity = context.Entry(entity);
            updateEntity.State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            using var context = new TContext();
            var updateEntity = context.Entry(entity);
            updateEntity.State = EntityState.Modified;
        await    context.SaveChangesAsync();
            return true;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            using var context = new TContext();
            return
                await context.Set<TEntity>().SingleOrDefaultAsync(expression);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
         using var context= new TContext();
            return context.Set<TEntity>().SingleOrDefault(expression);
        }

        public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression)
        {
          using var context=new TContext();
            return expression==null ?
                context.Set<TEntity>().ToListAsync()
                :context.Set<TEntity>().Where(expression).ToListAsync();

        }

     
    }
}
