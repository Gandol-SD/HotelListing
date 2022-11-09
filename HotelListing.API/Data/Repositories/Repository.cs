using HotelListing.API.Data.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApiDBX _dbx;

        public Repository(ApiDBX dbx)
        {
            _dbx = dbx;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbx.AddAsync(entity);
            await _dbx.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            if(entity != null)
            {
                _dbx.Set<T>().Remove(entity);
                await _dbx.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbx.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int? id)
        {
            if(id is null)
            {
                return null;
            }

            return await _dbx.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbx.Update(entity);
            await _dbx.SaveChangesAsync();
        }

        public async Task saveAsync()
        {
            await _dbx.SaveChangesAsync();
        }
    }
}
