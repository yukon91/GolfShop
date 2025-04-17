using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace GolfShopHemsida.Models
{
    //Dummy to ignore the need for email confirmation
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}