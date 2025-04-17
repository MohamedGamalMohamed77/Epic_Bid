using Epic_Bid.Core.Application.Abstraction.Models.Auth;
using Epic_Bid.Core.Application.Abstraction.Services.Auth;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Services.Auth
{
	public class EmailService(IOptions<EmailSettings> _emailSettings) : IEmailService
	{
		private readonly EmailSettings emailSettings = _emailSettings.Value;
		public async Task SendEmailAsync(EmailDto email)
		{
			var mail = new MimeMessage()
			{
				Sender = MailboxAddress.Parse(emailSettings.Email),
				Subject = email.Subject
			};
			mail.To.Add(MailboxAddress.Parse(email.To));
			mail.From.Add(new MailboxAddress(emailSettings.DisplayName,emailSettings.Email));
			var builder = new BodyBuilder();
			builder.TextBody = email.Body;
			mail.Body=builder.ToMessageBody();
			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(emailSettings.Host, emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
			await smtp.AuthenticateAsync(emailSettings.Email,emailSettings.Password);
			await smtp.SendAsync(mail);
			await smtp.DisconnectAsync(true);
		}
	}
}
