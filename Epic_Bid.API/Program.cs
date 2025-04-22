using Epic_Bid.API.Middlewares;
using Epic_Bid.Infrastructure.Persistence;
using Epic_Bid.Apis.Controllers.Controllers.Errors;
using Microsoft.AspNetCore.Mvc;
using Epic_Bid.Apis.Controllers;
using Epic_Bid.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Stripe;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Epic_Bid.Core.Domain.Contracts.Persistence;
using Epic_Bid.API.Extensions;
using Epic_Bid.Infrastructure.Persistence._Identity.Config;
using Epic_Bid.Core.Application;
using Epic_Bid.Core.Application.Mapping;

namespace Epic_Bid.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{

			var builder = WebApplication.CreateBuilder(args);


			#region Configure Services


			// Add services to the container.

			builder.Services.AddControllers().AddApplicationPart(typeof(Apis.Controllers.AssemblyInformation).Assembly);

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services
				.AddControllers()
				.ConfigureApiBehaviorOptions(options =>
				{
					options.SuppressModelStateInvalidFilter = false;
					options.InvalidModelStateResponseFactory = (actionContext) =>
					{
						var errors = actionContext.ModelState.Where(P => P.Value!.Errors.Count > 0)
									   .Select(P => new ApiValidationErrorResponse.ValidationError()
									   {
										   Field = P.Key,
										   Errors = P.Value!.Errors.Select(E => E.ErrorMessage)
									   });
						return new BadRequestObjectResult(new ApiValidationErrorResponse()
						{
							Errors = errors
						});
					};
				});

			builder.Services.AddApplicationServices();
			
			builder.Services.AddPersistenceServices(builder.Configuration);
			builder.Services.AddIdentityServices(builder.Configuration);
			
            #endregion


            var app = builder.Build();
            #region Update DataBase Initializer
            await app.InitializeAsync();
            #endregion


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
			{
				app.UseMiddleware<ExceptionHandlerMiddleware>();
				app.UseSwagger();
				app.UseSwaggerUI();
			}


			app.UseHttpsRedirection();


			app.UseStatusCodePagesWithReExecute("/errors/{0}");
			app.UseStaticFiles();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
