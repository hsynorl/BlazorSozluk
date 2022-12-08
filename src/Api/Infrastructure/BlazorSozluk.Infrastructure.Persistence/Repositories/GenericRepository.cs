using BlazorSozluk.Api.Application.Interfaces;
using BlazorSozluk.Api.Domain.Models;
using BlazorSozluk.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext dbContext;
        protected DbSet<TEntity> entity => dbContext.Set<TEntity>();


        public GenericRepository(DbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        #region Add Methods



        public int Add(TEntity entity)
        {//doğru
            this.entity.Add(entity);
            return dbContext.SaveChanges();
        }

        public int Add(IEnumerable<TEntity> entities)
        {//doğru
            if (entities != null && !entities.Any())
            {
                return 0;
            }
            entity.AddRange(entities);
            return dbContext.SaveChanges();
        }

        public async Task<int> AddAsync(TEntity entity)
        { //doğru
            await this.entity.AddAsync(entity);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> AddAsync(IEnumerable<TEntity> entities)
        {//doğru
            if (entities != null && !entities.Any())
            {
                return 0;
            }
            await entity.AddRangeAsync(entities);
            return await dbContext.SaveChangesAsync();
        }

        public int AddOrUpdate(TEntity entity)
        {//doğru
            if (!this.entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
            {
                dbContext.Update(entity);
            }
            return dbContext.SaveChanges();
        }

        public Task<int> AddOrUpdateAsync(TEntity entity)
        { //doğru
            if (!this.entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
            {
                dbContext.Update(entity);
            }
            return dbContext.SaveChangesAsync();
        }
        #endregion


        #region Delete Methods

        public int Delete(TEntity entity)
        {//doğru
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entity);
            }
            this.entity.Remove(entity);
            return dbContext.SaveChanges();
        }

        public Task<int> DeleteAsync(Guid id)
        {//doğru
            var entity = this.entity.Find(id);
            return DeleteAsync(entity);
        }

        public int Delete(Guid id)
        {//doğru
            var entity = this.entity.Find(id);
            return Delete(entity);
        }

        public Task<int> DeleteAsync(TEntity entities)
        {//doğru
            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entities);
            }
            this.entity.Remove(entities);
            return dbContext.SaveChangesAsync();
        }

        public bool DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {//doğru
            dbContext.RemoveRange(entity.Where(predicate));
            return dbContext.SaveChanges() > 0;
        }

        public async Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {//doğru
            dbContext.RemoveRange(entity.Where(predicate));
            return await dbContext.SaveChangesAsync() > 0;
        }

        #endregion

        #region Get Methods

        public IQueryable<TEntity> AsQueryable() => entity.AsQueryable();

        public Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {//doğru
            return Get(predicate, noTracking, includes).FirstOrDefaultAsync();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        { //doğru
            var query = entity.AsQueryable();
            if (predicate != null)
            {
                query = query.Where(predicate);

            }
            query = ApplayIncludes(query, includes);
            if (noTracking)
            {
                query = query.AsNoTracking();

            }
            return query;
        }

        public async Task<List<TEntity>> GetAll(bool noTracking = true)
        {//doğru
            if (noTracking)
            {
                return await entity.AsNoTracking().ToListAsync();
            }

            return await entity.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {//doğru
            TEntity found = await entity.FindAsync(id);
            if (found == null)
            {
                return null;
            }
            if (noTracking)
            {
                dbContext.Entry(found).State = EntityState.Detached;
            }

            foreach (Expression<Func<TEntity, object>> include in includes)
            {
                dbContext.Entry(found).Reference(include).Load();
            }
            return found;
        }

        public Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {//doğru
            IQueryable<TEntity> query = entity;
            if (predicate != null)
            {
                query = query.Where(predicate);

            }
            foreach (Expression<Func<TEntity, object>> include in includes)
            {
                query = query.Include(include);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (noTracking)
            {
                query = query.AsNoTracking();
            }
            return query.ToListAsync();
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {//doğru
            IQueryable<TEntity> query = entity;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            query = ApplayIncludes(query, includes);
            if (noTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.SingleOrDefaultAsync();
        }
        #endregion

        #region Update Methods  
        public int Update(TEntity entity)
        {//doğru
            this.entity.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            return dbContext.SaveChanges();
        }
        public async Task<int> UpdateAsync(TEntity entity)
        {//doğru
            this.entity.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            return await dbContext.SaveChangesAsync();
        }


        #endregion

        #region Bulk Method
        public Task BulkUpdate(IEnumerable<TEntity> entities)
        {//doğru
            if (entities != null && !entities.Any())
            {
                return Task.CompletedTask;
            }
            foreach (var entityItem in entities)
            {
                entity.Update(entityItem);
            }
            return dbContext.SaveChangesAsync();
        }
        public Task BulkDelete(Expression<Func<TEntity, bool>> predicate)
        {//doğru
            dbContext.RemoveRange(entity.Where(predicate));
            return dbContext.SaveChangesAsync();
        }

        public Task BulkDelete(IEnumerable<TEntity> entities)
        {//doğru
            if (entities != null && !entities.Any())
            {
                return Task.CompletedTask;
            }
            entity.RemoveRange(entities);

            return dbContext.SaveChangesAsync();
        }

        public Task BulkDeleteById(IEnumerable<Guid> ids)
        {//doğru
            if (ids != null && !ids.Any())
            {
                return Task.CompletedTask;
            }
            dbContext.RemoveRange(entity.Where(i => ids.Contains(i.Id)));
            return dbContext.SaveChangesAsync();
        }
        public Task BulkAdd(IEnumerable<TEntity> entities)
        {//doğru
            if (entities != null && !entities.Any())
            {
                return Task.CompletedTask;
            }
            foreach (var entityItem in entities)
            {
                entity.Add(entityItem);
            }
            return dbContext.SaveChangesAsync();
        }

        #endregion

        #region Utilities Func

        private static IQueryable<TEntity> ApplayIncludes(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
        {//doğru
            if (includes != null)
            {

                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }
            return query;
        }

        public Task<int> SaveChangesAsync()//doğru
        {
            return dbContext.SaveChangesAsync();
        }

        public int SaveChanges()//doğru
        {
            return dbContext.SaveChanges();
        }




        #endregion
    }
}
