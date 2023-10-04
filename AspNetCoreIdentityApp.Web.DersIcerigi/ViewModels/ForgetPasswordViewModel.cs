﻿using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.DersIcerigi.ViewModels
{
    public class ForgetPasswordViewModel
    {


        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        [Display(Name = "Email:")]
        public string Email { get; set; }

    }
}
