using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Models
{
    public class HotelUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
