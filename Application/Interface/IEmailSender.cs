using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string userEmail, string emailSubject, string message);
    }
}