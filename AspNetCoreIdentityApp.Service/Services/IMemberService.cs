using AspNetCoreIdentityApp.Core.DersIcerigi.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentityApp.Service.DersIcerigi.Services
{
    public interface IMemberService
    {

        public Task<UserViewModel> GetUserViewModelByUserNameAsync(string userName);

        public Task LogOutAsync();

        public Task<bool> CheckPasswordAsync(string userName, string password);

        public Task<(bool, IEnumerable<IdentityError>?)> ChangePasswordAsync(string userName, string oldPassword, string newPassword);

        public Task<UserEditViewModel> GetUserEditViewModelAsync(string userName);


        public SelectList GetGenderSelectList();

        public Task<(bool, IEnumerable<IdentityError>?)> EditUserAsync(UserEditViewModel request, string userName);

        public List<ClaimViewModel> GetClaims(ClaimsPrincipal principal);

    }
}
