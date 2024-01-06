using AspNetCoreIdentityApp.Repository.DersIcerigi.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Web.DersIcerigi.CustomValidations
{
    public class UserValidator : IUserValidator<AppUser>
    {
        Task<IdentityResult> IUserValidator<AppUser>.ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
          
            var errors= new List<IdentityError>();
            var isDigit = int.TryParse(user.UserName![0].ToString(), out _);

            if (isDigit)
            {
                errors.Add(new() { Code = "UsernameContainsFirstLetterDigit", Description = "Kullanıcı adının ilk karakteri sayısal değer içeremez" });
            }

            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);

        }
    }
}
