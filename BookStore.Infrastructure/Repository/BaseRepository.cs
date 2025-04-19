using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Application.Interface.Repository;

namespace BookStore.Infrastructure.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _baseContext;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(DbContext context)
        {
            _baseContext = context;
            _dbSet = _baseContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }   
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _baseContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<bool> UpdateAsync(TEntity updatedEntity, int id)
        {
            var existingEntity = await GetByIdAsync(id); 

            if (existingEntity == null)
            {
                return false;
            }

            _baseContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);

        
            var changes = await _baseContext.SaveChangesAsync();
            return changes > 0;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                return false;
            }

            _dbSet.Remove(entity);
            var changes = await _baseContext.SaveChangesAsync();
            return changes > 0;
        }

    }
}
