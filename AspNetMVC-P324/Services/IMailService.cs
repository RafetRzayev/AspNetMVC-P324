using AspNetMVC_P324.Data;

namespace AspNetMVC_P324.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(RequestEmail requestEmail);
    }
}
