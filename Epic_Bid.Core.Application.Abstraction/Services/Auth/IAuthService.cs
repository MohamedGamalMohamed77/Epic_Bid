using Epic_Bid.Core.Application.Abstraction.Common;
using Epic_Bid.Core.Application.Abstraction.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Services.Auth
{
	public interface IAuthService
	{
		Task<UserDto> LoginAsync(LoginDto model);
		Task<UserDto> RegisterAsync(RegisterDto model);
		Task<UserDto> GetCurrentUser(ClaimsPrincipal claimsPrincipal);
		Task<AddressDto?> GetUserAddress(ClaimsPrincipal claimsPrincipal);
		Task<AddressDto> UpdateUserAddress(ClaimsPrincipal claimsPrincipal, AddressDto addressDto);
		Task<AddressDto> AddUserAddress(ClaimsPrincipal claimsPrincipal, AddressDto addressDto);
		Task<string> ForgetPasswordAsync(ForgetPasswordDto model);
		Task<string> VerifyCodeAsync(VerifyCodeDto model);
		Task<string> ResetPasswordAsync(ResetPasswordDto model);
		Task<bool> EmailExists(string email);
	}
}
