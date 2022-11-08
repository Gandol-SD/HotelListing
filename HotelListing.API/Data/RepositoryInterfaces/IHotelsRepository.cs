using HotelListing.API.Models;

namespace HotelListing.API.Data.RepositoryInterfaces
{
    public interface IHotelsRepository : IRepository<Hotel>
    {
        Task<Hotel> GetDetails(int id);
    }
}
