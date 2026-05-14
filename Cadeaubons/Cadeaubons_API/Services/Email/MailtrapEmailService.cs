using MailKit.Net.Smtp;
using MimeKit;
using Cadeaubons_API.Options;
using Cadeaubons_Domain.Email;
using Microsoft.Extensions.Options;

namespace Cadeaubons_API.Services.Email
{
	public class MailtrapEmailService : IEmailService
	{
		private readonly MailtrapOptions _options;

		public MailtrapEmailService(IOptions<MailtrapOptions> options)
		{
			_options = options.Value;
		}

		public async Task SendAsync(string to, string subject, string body)
		{
			var message = new MimeMessage();

			message.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
			message.To.Add(MailboxAddress.Parse(to));
			message.Subject = subject;

			message.Body = new TextPart("html")
			{
				Text = body
			};

			using var client = new SmtpClient();

			client.CheckCertificateRevocation = false;

			await client.ConnectAsync(_options.Host, _options.Port, MailKit.Security.SecureSocketOptions.StartTls);
			await client.AuthenticateAsync(_options.Username, _options.Password);


			await client.SendAsync(message);
			await client.DisconnectAsync(true);
		}
	}
}
