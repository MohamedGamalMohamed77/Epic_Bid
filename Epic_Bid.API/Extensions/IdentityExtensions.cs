using Epic_Bid.Core.Application.Abstraction.Models.Auth;
using Epic_Bid.Core.Application.Abstraction.Services.Auth;
using Epic_Bid.Core.Application.Abstraction.Services.Role;
using Epic_Bid.Core.Application.Services.Auth;
using Epic_Bid.Core.Application.Services.Role;
using Epic_Bid.Core.Domain.Entities.Auth;
using Epic_Bid.Core.Domain.Entities.Roles;
using Epic_Bid.Infrastructure.Persistence;
using Epic_Bid.Infrastructure.Persistence._IdentityAndData.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Epic_Bid.API.Extensions
{
    public static class IdentityExtensions
	{
		public static IServiceCollection AddIdentityServices(this IServiceCollection services , IConfiguration configuration)
		{
			services.Configure<JwtSettings>(configuration.GetSection("JWTSettings"));

			services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));



			services.AddIdentity<ApplicationUser, AppRole>((IdentityOptions) =>
			{
				#region Sign In
				//IdentityOptions.SignIn.RequireConfirmedAccount = true;
				//IdentityOptions.SignIn.RequireConfirmedEmail = true;
				//IdentityOptions.SignIn.RequireConfirmedPhoneNumber = true; 
				#endregion


				#region Password validation
				//IdentityOptions.Password.RequiredUniqueChars = 2;
				//IdentityOptions.Password.RequireNonAlphanumeric = true;
				//IdentityOptions.Password.RequiredLength = 6;
				//IdentityOptions.Password.RequireLowercase = true;
				//IdentityOptions.Password.RequireUppercase = true;
				//IdentityOptions.Password.RequireDigit = true; 
				#endregion

				IdentityOptions.User.RequireUniqueEmail = true;

				IdentityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
				IdentityOptions.Lockout.AllowedForNewUsers = true;
				IdentityOptions.Lockout.MaxFailedAccessAttempts = 5;



			}).
			AddEntityFrameworkStores<StoreIdentityDbContext>();

			
                
            services.AddAuthentication((authenticationOptions) =>
			{
				authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer((options) =>
				{
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,

						ValidIssuer = configuration["JWTSettings:Issuer"],
						ValidAudience = configuration["JWTSettings:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]!)),
						ClockSkew = TimeSpan.Zero

					};

				});

			services.AddScoped(typeof(IAuthService), typeof(AuthService));
            // Add IRoleService 
            services.AddScoped(typeof(IRoleService), typeof(RoleService));
            // IUserService
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddTransient(typeof(IEmailService), typeof(EmailService));
			services.AddScoped(typeof(Func<IAuthService>), (serviceProvider) =>
			{
				return  () => serviceProvider.GetRequiredService<IAuthService>();
			});

			return services;

			
		}
		
	}
}
