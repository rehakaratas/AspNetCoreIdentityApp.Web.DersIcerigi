﻿using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Core.DersIcerigi.ViewModels
{
    public class SignInViewModel
    {

        public SignInViewModel()
        {
                
        }
        public SignInViewModel(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [Required(ErrorMessage ="Email alanı boş bırakılamaz")]
        [EmailAddress(ErrorMessage ="Email formatı yanlıştır")]
        [Display(Name ="Email :")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Şifre alanı boş bırakılamaz")]
        [Display(Name ="Şifre :")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir.")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla ")]
        public bool RememberMe { get; set; }



    }
}
