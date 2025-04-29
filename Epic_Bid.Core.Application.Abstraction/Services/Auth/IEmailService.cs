using Epic_Bid.Core.Application.Abstraction.Models.Auth;
using Epic_Bid.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Services.Auth
{
	public interface IEmailService
	{
		public Task SendEmailAsync(EmailDto email);
		public Task SendEmailToWinnerAsync(string toemail, string subject, EmailWinnerDataDto emailWinnerData);

    }
}
