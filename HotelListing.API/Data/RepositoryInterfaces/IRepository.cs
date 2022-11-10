using HotelListing.API.Models;

namespace HotelListing.API.Data.RepositoryInterfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(int? id);
        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParams queryParams);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

        Task<bool> Exists(int id);

        Task saveAsync();
    }
}
