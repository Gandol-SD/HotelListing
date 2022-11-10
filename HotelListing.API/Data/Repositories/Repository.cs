using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Data.RepositoryInterfaces;
using HotelListing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApiDBX _dbx;
        private readonly IMapper mapper;

        public Repository(ApiDBX dbx, IMapper mapper)
        {
            _dbx = dbx;
            this.mapper = mapper;
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

        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParams queryParams)
        {
            var totalSize = await _dbx.Set<T>().CountAsync();
            var items = await _dbx.Set<T>().Skip(queryParams.StartIndex)
                .Take(queryParams.PageSize)
                .ProjectTo<TResult>(mapper.ConfigurationProvider)
                .ToListAsync();
            return new PagedResult<TResult>
            {
                Items = items,
                pageNumber = queryParams.pageNumber,
                recordNumber = queryParams.PageSize,
                totalCount = totalSize
            };
        }
    }
}
