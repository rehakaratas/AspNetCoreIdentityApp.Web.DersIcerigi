using AspNetCoreIdentityApp.Web.DersIcerigi.CustomValidations;
using AspNetCoreIdentityApp.Web.DersIcerigi.Localizations;
using AspNetCoreIdentityApp.Web.DersIcerigi.Models;

namespace AspNetCoreIdentityApp.Web.DersIcerigi.Extensions
{
    public static class StartupExtensions
    {

        public static void AddIdentityWithExtension(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz1234567890=";

                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                options.Lockout.MaxFailedAccessAttempts = 3;

            }).AddPasswordValidator<PasswordValidator>().AddUserValidator<UserValidator>().AddErrorDescriber<LocalizationIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbContext>();
        }
    }
}
