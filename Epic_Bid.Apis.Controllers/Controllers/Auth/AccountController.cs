using Epic_Bid.Apis.Controllers.Controllers.Base;
using Epic_Bid.Core.Application.Abstraction.Services;
using Epic_Bid.Core.Application.Abstraction.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epic_Bid.Core.Application.Abstraction.Models.Auth;
using Epic_Bid.Core.Application.Abstraction.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;

namespace Epic_Bid.Apis.Controllers.Controllers.Auth
{
	public class AccountController(IServiceManager serviceManager) : BaseApiController
	{
		[HttpPost("Login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var response = await serviceManager.AuthService.LoginAsync(model);
			return Ok(response);
		}

		[HttpPost("Register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{
			var response = await serviceManager.AuthService.RegisterAsync(model);
			return Ok(response);
		}
		[Authorize]
		[HttpGet]
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var result = await serviceManager.AuthService.GetCurrentUser(User);
			return Ok(result);
		}

		[Authorize]
		[HttpGet("address")]
		public async Task<ActionResult<AddressDto>> GetUserAddress()
		{
			var result = await serviceManager.AuthService.GetUserAddress(User);
			return Ok(result);
		}
		[Authorize]
		[HttpPost("address")]
		public async Task<ActionResult<AddressDto>> AddUserAddress(AddressDto addressDto)
		{
			var result = await serviceManager.AuthService.AddUserAddress(User, addressDto);
			return Ok(result);
		}
		[Authorize]
		[HttpPut("address")]
		public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
		{
			var result = await serviceManager.AuthService.UpdateUserAddress(User, addressDto);
			return Ok(result);
		}
		[Authorize]
		[HttpGet("emailexists")]
		public async Task<ActionResult<bool>> CheckEmailExists(string email)
		{
			var result = await serviceManager.AuthService.EmailExists(email!);
			return Ok(result);

		}
		[HttpPost("forgetPassword")]
		public async Task<ActionResult<string>> ForgetPassword(ForgetPasswordDto email)
		{
			var response = await serviceManager.AuthService.ForgetPasswordAsync(email);
			return Ok(response);
		}

		[HttpPost("verifyCode")]
		public async Task<ActionResult<string>> VerifyCode(VerifyCodeDto email)
		{
			var response = await serviceManager.AuthService.VerifyCodeAsync(email);
			return Ok(response);
		}

		[HttpPut("resetPassword")]
		public async Task<ActionResult<string>> ResetPassword(ResetPasswordDto email)
		{
			var response = await serviceManager.AuthService.ResetPasswordAsync(email);
			return Ok(response);
		}
	}
}
