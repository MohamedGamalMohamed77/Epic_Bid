using AutoMapper;
using Epic_Bid.Core.Application.Abstraction.Common;
using Epic_Bid.Core.Application.Abstraction.Models.Auth;
using Epic_Bid.Core.Application.Abstraction.Services.Auth;
using Epic_Bid.Core.Application.Exceptions;
using Epic_Bid.Core.Application.Extensions;
using Epic_Bid.Core.Domain.Entities.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Epic_Bid.Core.Application.Services.Auth
{
    public class AuthService(
		IEmailService emailService,
		IMapper mapper,
		IOptions<JwtSettings> jwtSettings,
		UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager) : IAuthService
	{
		private readonly JwtSettings _jwtSettings = jwtSettings.Value;

		public async Task<UserDto> GetCurrentUser(ClaimsPrincipal claimsPrincipal)
		{
			var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

			var user = await userManager.FindByEmailAsync(email!);

			return new UserDto()
			{
				Id = user!.Id,
				DisplayName = user!.DisplayName,
				Email = user.Email!,
				Token = await GenerateTokenAsync(user),
			};

		}

		public async Task<AddressDto?> GetUserAddress(ClaimsPrincipal claimsPrincipal)
		{
			var user = await userManager.FindUserWithAddress(claimsPrincipal);
			var address = mapper.Map<AddressDto?>(user?.Address);
			return address;
		}
		public async Task<AddressDto> AddUserAddress(ClaimsPrincipal claimsPrincipal, AddressDto addressDto)
		{
			var user = await userManager.FindUserWithAddress(claimsPrincipal);
			if (user == null)
				throw new BadRequestException("User not found");

			if (user.Address != null)
				throw new BadRequestException("User already has an address. Use update instead.");

			var address = mapper.Map<Address>(addressDto);

			user.Address = address;

			var result = await userManager.UpdateAsync(user);
			if (!result.Succeeded)
				throw new BadRequestException(result.Errors
				  .Select(e => e.Description)
				  .Aggregate((x, y) => $"{x}, {y}"));

			return addressDto;
		}
		public async Task<AddressDto> UpdateUserAddress(ClaimsPrincipal claimsPrincipal, AddressDto addressDto)
		{
			var address = mapper.Map<Address>(addressDto);

			var user = await userManager.FindUserWithAddress(claimsPrincipal);
			if (user?.Address != null)
				address.Id = user.Address.Id;

			user!.Address = address;

			var result = await userManager.UpdateAsync(user);

			if (!result.Succeeded)
				throw new BadRequestException(result.Errors.Select(u => u.Description).Aggregate((x, y) => $"{x} , {y}"));
			return addressDto;

		}

	
		public async Task<UserDto> LoginAsync(LoginDto model)
		{
			var user = await userManager.FindByEmailAsync(model.Email);
			if (user is null) throw new UnAuthorizedException("Invalid Login");

			var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
			if (!result.Succeeded) throw new UnAuthorizedException("Password Wrong");

			var response = new UserDto()
			{
				Id = user.Id,
				DisplayName = user.DisplayName,
				Email = user.Email!,
				Token = await GenerateTokenAsync(user),

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
			if (!result.Succeeded) throw new ValidationException() { Errors = result.Errors.Select(E => E.Description) };

			var response = new UserDto()
			{
				Id = user.Id,
				DisplayName = user.DisplayName,
				Email = user.Email!,
				Token = await GenerateTokenAsync(user),

			};
			return response;

		}

		private async Task<string> GenerateTokenAsync(ApplicationUser user)
		{


			var privateClaims = new List<Claim>()
		{
		  new Claim(ClaimTypes.PrimarySid,user.Id),
		  new Claim(ClaimTypes.Email,user.Email!),
		  new Claim(ClaimTypes.GivenName,user.DisplayName),
		}.Union(await userManager.GetClaimsAsync(user)).ToList();

			foreach (var role in await userManager.GetRolesAsync(user))
				privateClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));

			var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

			var tokenObj = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				expires: DateTime.UtcNow.AddHours(_jwtSettings.DurationInHours),
		claims: privateClaims,
					signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
					);
			return new JwtSecurityTokenHandler().WriteToken(tokenObj);
		}
		public async Task<bool> EmailExists(string email)
		{
			return await userManager.FindByEmailAsync(email) is not null;
		}

		public async Task<string> ForgetPasswordAsync(ForgetPasswordDto model)
		{
			var user = await userManager.FindByEmailAsync(model.Email);

			if (user is null)
				throw new BadRequestException("Email not found");

			user.ResetCode = RandomNumberGenerator.GetInt32(100_0, 999_9);

			user.ResetCodeExpire = DateTime.UtcNow.AddMinutes(15);



			var result = await userManager.UpdateAsync(user);

			if (!result.Succeeded)
				throw new BadRequestException("may be there is a problem  try again");

			var Email = new EmailDto()
			{
				To = model.Email,
				Subject = $" Hi {user.DisplayName} Reset Code For EpicBid Account",
				Body = $"We Have Recived Your Request For Reset Your Account Password, \nYour Reset Code Is ==> [ {user.ResetCode} ] <== \nNote: This Code Will Be Expired After 15 Minutes!",
			};

			await emailService.SendEmailAsync(Email);

			return user.ResetCode.ToString();
		}

		public async Task<string> VerifyCodeAsync(VerifyCodeDto model)
		{
			var user = await userManager.FindByEmailAsync(model.Email);

			if (user is null)
				throw new BadRequestException("Email not found ..!");

			if (user.ResetCode != model.ResetCode)
				throw new BadRequestException("the code is false ..!");

			if (user.ResetCodeExpire < DateTime.UtcNow)
				throw new BadRequestException("the code is expired because it exceede 15 minutes  try again ");


			return ("Operation completed successfully.");
				
		}

		public async Task<string> ResetPasswordAsync(ResetPasswordDto model)
		{
			var user = await userManager.FindByEmailAsync(model.Email);

			if (user is null)
				throw new BadRequestException("Email not found ..!");

			var RemovePass = await userManager.RemovePasswordAsync(user);

			if (!RemovePass.Succeeded)
				throw new BadRequestException("something went wrong try again ");

			var newPass = await userManager.AddPasswordAsync(user, model.NewPassword);

			if (!newPass.Succeeded)
				throw new BadRequestException("something went wrong try again ");

			return ("Operation completed successfully.");
		}
		
	}
}
