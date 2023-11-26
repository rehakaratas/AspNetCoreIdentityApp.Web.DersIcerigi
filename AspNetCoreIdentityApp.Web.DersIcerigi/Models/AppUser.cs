using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.DersIcerigi.Models
{
    public class AppUser:IdentityUser 
    {
        public string? City { get; set; }
        public string? Picture { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }

    }
}
