using HotelListing.API.Data.RepositoryInterfaces;
using HotelListing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data.Repositories
{
    public class HotelsRepository : Repository<Hotel>, IHotelsRepository
    {
        public HotelsRepository(ApiDBX _dbx) : base(_dbx)
        {
            dbx = _dbx;
        }

        public ApiDBX dbx { get; }

        public async Task<Hotel> GetDetails(int id)
        {
            return await dbx.Hotels.Include(q => q.Country).FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
