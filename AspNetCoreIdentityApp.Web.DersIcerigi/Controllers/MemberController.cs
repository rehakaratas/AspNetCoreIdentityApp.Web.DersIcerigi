using AspNetCoreIdentityApp.Web.DersIcerigi.Extensions;
using AspNetCoreIdentityApp.Web.DersIcerigi.Models;
using AspNetCoreIdentityApp.Web.DersIcerigi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.DersIcerigi.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {

        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }

        public async Task<IActionResult> Index()
        {

            var userClaims=User.Claims.ToList();

            var currentUser =(await _userManager.FindByNameAsync(User.Identity!.Name!))!;
            var userViewMoodel = new UserViewModel
            {
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                PhoneNumber = currentUser.PhoneNumber,
                PictureUrl=currentUser.Picture
            };

             
            return View(userViewMoodel);
        }

        public async Task LogOut()
        {

            await _signInManager.SignOutAsync();

            
        }

        public IActionResult PasswordChange()
        {


            return View();


        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.PasswordOld);

            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
                return View();
            }

            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, request.PasswordOld,request.PasswordNew);

            if(!resultChangePassword.Succeeded)
            {
                ModelState.AddModelErrorList(resultChangePassword.Errors);

                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, request.PasswordNew, true, false);

            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirilmiştir";

            return View();


        }


        public async Task<IActionResult> UserEdit()
        {
            ViewBag.genderList=new SelectList(Enum.GetNames(typeof(Gender)));

            var currentUser=(await _userManager.FindByNameAsync(User.Identity!.Name!))!;

            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName!,
                Email = currentUser.Email!,
                Phone = currentUser.PhoneNumber!,
                BirthDate = currentUser.BirthDate,
                City = currentUser.City,
                Gender = currentUser.Gender
            };


            return View(userEditViewModel);
        }


        [HttpPost]
        public async Task<IActionResult>UserEdit(UserEditViewModel request)
        {


            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser =await _userManager.FindByNameAsync(User.Identity!.Name!);

            currentUser.UserName=request.UserName;
            currentUser.Email=request.Email;
            currentUser.PhoneNumber = request.Phone;
            currentUser.BirthDate = request.BirthDate;
            currentUser.City = request.City;
            currentUser.Gender = request.Gender;


            if(request.Picture != null&&request.Picture.Length>0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");

                string randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}";

                var newPicturePath = Path.Combine(wwwrootFolder!.First(x => x.Name == "userpictures").PhysicalPath!, randomFileName);


                using var stream = new FileStream(newPicturePath, FileMode.Create);

                await request.Picture.CopyToAsync(stream);

                currentUser.Picture = randomFileName;

            }

            var updateToUserResult=await _userManager.UpdateAsync(currentUser);

            if(!updateToUserResult.Succeeded)
            {
                ModelState.AddModelErrorList(updateToUserResult.Errors);

                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);

            await _signInManager.SignOutAsync();

            if(request.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(currentUser, true, new[] { new Claim("birthdate", currentUser.BirthDate!.Value.ToString()) });
            }

            else
            {
                await _signInManager.SignInAsync(currentUser, true);
            }

            
            TempData["SuccessMessage"] = "Üye bilgileri başarıyla değiştirilmiştir.";


            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName!,
                Email = currentUser.Email!,
                Phone = currentUser.PhoneNumber!,
                BirthDate = currentUser.BirthDate,
                City = currentUser.City,
                Gender = currentUser.Gender
            };

            return View(userEditViewModel);


        }

        [HttpGet]
        public IActionResult Claims()
        {

            var userClaimList = User.Claims.Select(x => new ClaimViewModel()
            {
                Issuer = x.Issuer,
                Type = x.Type,
                Value = x.Value
            }).ToList();

            return View(userClaimList);
        }

        [Authorize(Policy ="AnkaraPolicy")]
        [HttpGet]
        public IActionResult AnkaraPage()
        {
            return View();
        }


        [Authorize(Policy = "ExchangePolicy")]
        [HttpGet]
        public IActionResult ExchangePage()
        {
            return View();
        }

        [Authorize(Policy = "ViolencePolicy")]
        [HttpGet]
        public IActionResult ViolencePage()
        {
            return View();
        }

        public async Task<IActionResult> AccessDenied(string returnUrl)
        {

            string message=string.Empty;


            message = "Bu sayfayı görmeye yetkiniz yoktur. Yetki almak için yöneticinizle görüşebilirsiniz.";

            ViewBag.message=message;

            return View();
        }
    }
}
