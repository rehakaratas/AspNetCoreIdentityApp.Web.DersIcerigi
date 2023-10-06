using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.DersIcerigi.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [Display(Name = "Yeni Şifre:")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Şifreler aynı değildir.")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz.")]
        [Display(Name = "Yeni Şifre Tekrar:")]
        public string PasswordConfirm { get; set; }


    }
}
