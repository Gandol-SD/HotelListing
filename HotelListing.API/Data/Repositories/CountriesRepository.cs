using HotelListing.API.Data.RepositoryInterfaces;
using HotelListing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data.Repositories
{
    public class CountriesRepository : Repository<Country>, ICountriesRepository
    {
        public ApiDBX dbx { get; }

        public CountriesRepository(ApiDBX _dbx) : base(_dbx)
        {
            dbx = _dbx;
        }

        public async Task<Country> GetDetails(int id)
        {
            return await dbx.Countries.Include(q => q.Hotels).FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
