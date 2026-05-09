using Cadeaubons_API.Options;
using Cadeaubons_API.Services.Email;
using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Email;
using Microsoft.Extensions.Options;

namespace emailtestapp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var dto = new VoucherDTO
            {
                UserFullName = "Jan Jansen",
                BuyerFullName = "Piet Pieterson",
                ThemeName = "Verjaardag",
                InitialAmount = 50,
                PurchaseDate = DateTime.Now
            };

            string body = EmailTemplateService.GetVoucherGiftEmailBody(dto, "ABC123");

            var options = Options.Create(new MailtrapOptions
            {
                Host = "sandbox.smtp.mailtrap.io",
                Port = 587,
                Username = "ae438301780fac",
                Password = "55a5534b805472",
                FromEmail = "noreply@cadeaubonapp.com",
                FromName = "Cadeaubon App"
            });

            //IEmailService emailService = new MailtrapEmailService(options);

            //await emailService.SendAsync(
            //    "test@mailtrap.io",
            //    "🎁 Test email",
            //    body
            //);
        }
    }
}
