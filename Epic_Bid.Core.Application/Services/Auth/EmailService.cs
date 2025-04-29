using Epic_Bid.Core.Application.Abstraction.Models.Auth;
using Epic_Bid.Core.Application.Abstraction.Services.Auth;
using Epic_Bid.Core.Domain.Entities.Products;
using Epic_Bid.Shared;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Services.Auth
{
    public class EmailService(IOptions<EmailSettings> _emailSettings, IWebHostEnvironment _env, IConfiguration _configeration) : IEmailService
	{
		private readonly EmailSettings emailSettings = _emailSettings.Value;
		#region Send Email Async
		public async Task SendEmailAsync(EmailDto email)
		{
			var mail = new MimeMessage()
			{
				Sender = MailboxAddress.Parse(emailSettings.Email),
				Subject = email.Subject
			};
			mail.To.Add(MailboxAddress.Parse(email.To));
			mail.From.Add(new MailboxAddress(emailSettings.DisplayName, emailSettings.Email));
			var builder = new BodyBuilder();
			builder.TextBody = email.Body;
			mail.Body = builder.ToMessageBody();
			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(emailSettings.Host, emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
			await smtp.AuthenticateAsync(emailSettings.Email, emailSettings.Password);
			await smtp.SendAsync(mail);
			await smtp.DisconnectAsync(true);
		}
        #endregion

        #region Email To Winner
        public async Task SendEmailToWinnerAsync(string toemail, string subject, EmailWinnerDataDto emailWinnerData)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configeration["EmailSettings:DisplayName"], _configeration["EmailSettings:Email"]));
            message.To.Add(MailboxAddress.Parse(toemail));
            message.Subject = subject;

            var builder = new BodyBuilder();

            var filePath = Path.Combine(_env.WebRootPath,"TemplateHtml", "WinnerAuciton.html"); // Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TemplateHtml", "WinnerAuciton.html");
            Console.WriteLine(filePath);
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Email template not found at {filePath}");

            var body = File.ReadAllText(filePath);
            string fullBody = body
                .Replace("{{UserName}}", emailWinnerData.Username)
                .Replace("{{ProductName}}", emailWinnerData.Productname)
                .Replace("{{FinalPrice}}", emailWinnerData.Finlaprice.ToString("N2"))
                .Replace("{{Auction To End}}", emailWinnerData.AuctionEndDate?.ToString("dd/MM/yyyy hh:mm tt"));

            builder.HtmlBody = fullBody;
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(_configeration["EmailSettings:Host"], Convert.ToInt32(_configeration["EmailSettings:Port"]), true);
                await client.AuthenticateAsync(_configeration["EmailSettings:Email"], _configeration["EmailSettings:Password"]);

                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw; // مهم عشان تعرف لو فيه مشكلة
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        #endregion   
    }
}
