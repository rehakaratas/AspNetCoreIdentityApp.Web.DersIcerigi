namespace AspNetCoreIdentityApp.Service.DersIcerigi.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail);
        

    }
}
