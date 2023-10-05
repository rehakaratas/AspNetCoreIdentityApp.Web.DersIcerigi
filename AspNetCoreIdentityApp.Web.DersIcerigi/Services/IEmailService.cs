namespace AspNetCoreIdentityApp.Web.DersIcerigi.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail);
        

    }
}
