using HotelListing.API.Data.RepositoryInterfaces;
using HotelListing.API.Models;

namespace HotelListing.API.Data.Repositories
{
    public class CountriesRepository : Repository<Country>, ICountriesRepository
    {
        public CountriesRepository(ApiDBX dbx) : base(dbx)
        {

        }
    }
}
