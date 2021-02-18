using System.Threading.Tasks;
using Application.Interface;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<SendGridSettings> _settings;

        public EmailSender(IOptions<SendGridSettings> settings)
        {
            _settings = settings;
        }

        public async Task SendEmailAsync(string userEmail, string emailSubject, string message)
        {
            //Creates the connection between our system and SendGrid API
            var client = new SendGridClient(_settings.Value.Key);

            //Creates the message and specified the address thta sent the mail
            var msg = new SendGridMessage
            {
                From = new EmailAddress("antoineheb@outlook.com", _settings.Value.User),
                Subject = emailSubject,
                PlainTextContent = message,
                HtmlContent = message
            };
            
            msg.AddTo(new EmailAddress(userEmail));
            msg.SetClickTracking(false, false);

            await client.SendEmailAsync(msg);
        }
    }
}