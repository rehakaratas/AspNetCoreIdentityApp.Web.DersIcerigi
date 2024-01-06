using AspNetCoreIdentityApp.Core.DersIcerigi.Models;
using AspNetCoreIdentityApp.Core.DersIcerigi.ViewModels;
using AspNetCoreIdentityApp.Repository.DersIcerigi.Models;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentityApp.Service.DersIcerigi.Services
{
    public class MemberService : IMemberService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IFileProvider _fileProvider;

        public MemberService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IFileProvider fileProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _fileProvider = fileProvider;
        }

        public async Task LogOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        async Task<UserViewModel> IMemberService.GetUserViewModelByUserNameAsync(string userName)
        {
            var currentUser = (await _userManager.FindByNameAsync(userName))!;
            return new UserViewModel
            {
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                PhoneNumber = currentUser.PhoneNumber,
                PictureUrl = currentUser.Picture
            };
        }

        async Task<bool> IMemberService.CheckPasswordAsync(string userName, string password)
        {
            var currentUser = (await _userManager.FindByNameAsync(userName))!;

            return await _userManager.CheckPasswordAsync(currentUser, password);
        }


        async Task<(bool,IEnumerable<IdentityError>?)> IMemberService.ChangePasswordAsync(string userName,string oldPassword,string newPassword)
        {

            var currentUser = (await _userManager.FindByNameAsync(userName))!;

            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, oldPassword, newPassword);

            if(!resultChangePassword.Succeeded)
            {
                return (false,resultChangePassword.Errors);
            }


            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, newPassword, true, false);

            return (true,null);

        }


        async Task<UserEditViewModel> IMemberService.GetUserEditViewModelAsync(string userName)
        {
            var currentUser = (await _userManager.FindByNameAsync(userName))!;

            return new UserEditViewModel()
            {
                UserName = currentUser.UserName!,
                Email = currentUser.Email!,
                Phone = currentUser.PhoneNumber!,
                BirthDate = currentUser.BirthDate,
                City = currentUser.City,
                Gender = currentUser.Gender
            };
        }

        SelectList IMemberService.GetGenderSelectList()
        {
            return new SelectList(Enum.GetNames(typeof(Gender)));
        }



        async Task<(bool, IEnumerable<IdentityError>?)> IMemberService.EditUserAsync(UserEditViewModel request,string userName)
        {

            var currentUser = (await _userManager.FindByNameAsync(userName))!;

            currentUser.UserName = request.UserName;
            currentUser.Email = request.Email;
            currentUser.PhoneNumber = request.Phone;
            currentUser.BirthDate = request.BirthDate;
            currentUser.City = request.City;
            currentUser.Gender = request.Gender;


            if (request.Picture != null && request.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                string randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}";

                var newPicturePath = Path.Combine(wwwrootFolder!.First(x => x.Name == "userpictures").PhysicalPath!, randomFileName);


                using var stream = new FileStream(newPicturePath, FileMode.Create);

                await request.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;

            }


            var updateToUserResult = await _userManager.UpdateAsync(currentUser);

            if(!updateToUserResult.Succeeded)
            {
                return (false, updateToUserResult.Errors);
            }


            await _userManager.UpdateSecurityStampAsync(currentUser);

            await _signInManager.SignOutAsync();


            if (request.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(currentUser, true, new[] { new Claim("birthdate", currentUser.BirthDate!.Value.ToString()) });
            }

            else
            {
                await _signInManager.SignInAsync(currentUser, true);
            }

            return (true, null);

        }


        List<ClaimViewModel> IMemberService.GetClaims(ClaimsPrincipal principal)
        {

            return principal.Claims.Select(x => new ClaimViewModel()
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value
            }).ToList();
        }

    }
}
