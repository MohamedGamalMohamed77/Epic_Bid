using Epic_Bid.Core.Application.Abstraction.Models.Auth;
using Epic_Bid.Core.Application.Abstraction.Services.Auth;
using Epic_Bid.Core.Application.Exceptions;
using Epic_Bid.Core.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Epic_Bid.Core.Application.Services
{
	public class AuthService(UserManager<ApplicationUser> userManager ,SignInManager<ApplicationUser> signInManager ) : IAuthService
	{
		public async Task<UserDto> LoginAsync(LoginDto model)
		{
			var user = await userManager.FindByEmailAsync(model.Email);
			if (user is null) throw new BadRequestException("Invalid Login");
			
			var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
			if (!result.Succeeded) throw new BadRequestException("Password Wrong");
			
			var response = new UserDto()
			{
				Id=user.Id,
				DisplayName =user.DisplayName,
				Email=user.Email!,
				Token="Jwt Token"
			
			};
			return response;
		}

		public async Task<UserDto> RegisterAsync(RegisterDto model)
		{
			var user = new ApplicationUser()
			{

				DisplayName = model.DisplayName,
				Email = model.Email,
				PhoneNumber = model.Phone,
				UserName = model.Email,
			};
			
			var result = await userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded) throw new ValidationException() { Errors = result.Errors.Select(E=>E.Description)};

			var response = new UserDto()
			{
				Id = user.Id,
				DisplayName = user.DisplayName,
				Email = user.Email!,
				Token = "Jwt Token"

			};
			return response;

		}
	}
}
