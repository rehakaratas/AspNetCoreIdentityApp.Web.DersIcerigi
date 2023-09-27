using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.DersIcerigi.Models
{
    public class AppUser:IdentityUser 
    {
        public string City { get; set; }

    }
}
