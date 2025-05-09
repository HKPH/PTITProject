﻿namespace BookStore.Application.Interface.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity, int id);
        Task<bool> DeleteAsync(int id);
    }
}
