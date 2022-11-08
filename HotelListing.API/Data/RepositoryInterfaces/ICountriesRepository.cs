using HotelListing.API.Models;

namespace HotelListing.API.Data.RepositoryInterfaces
{
    public interface ICountriesRepository : IRepository<Country>
    {
        Task<Country> GetDetails(int id);
    }
}
