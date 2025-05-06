using Epic_Bid.API.Extensions;
using Epic_Bid.API.Middlewares;
using Epic_Bid.Apis.Controllers.Controllers.Errors;
using Epic_Bid.Core.Application;
using Epic_Bid.Infrastructure;
using Epic_Bid.Infrastructure.Persistence;
using Epic_Bid.Shared.HangFire;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Epic_Bid.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{

			var builder = WebApplication.CreateBuilder(args);


			#region Configure Services


			// Add services to the container.
			#region Add Hangfire
			builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("StoreIdentityContext")));
			builder.Services.AddHangfireServer();
			#endregion


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
				})
				.AddApplicationPart(typeof(Apis.Controllers.AssemblyInformation).Assembly)
				.AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			})
				;


			builder.Services.AddApplicationServices();

			builder.Services.AddPersistenceServices(builder.Configuration);
			builder.Services.AddInfrastructureServices(builder.Configuration);
			builder.Services.AddIdentityServices(builder.Configuration);

			builder.Services.AddCors(corsOptions =>
						{
							corsOptions.AddPolicy("EpicBidPolicy", policyBuilder =>
							{
								policyBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();

							});
						});

			builder.Services.AddSwaggerGen(c =>
			{
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				if (File.Exists(xmlPath))
				{
					c.IncludeXmlComments(xmlPath);
				}
			});
			#endregion

			var app = builder.Build();

			#region Update Database
			try
			{
				await app.InitializeAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Startup Error: " + ex.Message);
				throw;
			}

			#endregion


			// Configure the HTTP request pipeline.
			app.UseMiddleware<ExceptionHandlerMiddleware>();

			if (app.Environment.IsDevelopment())
			{

				app.UseSwagger(); 
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "Epic Bid API V1");
				});
			}
			app.UseHttpsRedirection();
			//app.UseStatusCodePagesWithReExecute("/errors/{0}");
			app.UseStaticFiles();

			app.UseCors("EpicBidPolicy");

			app.UseAuthentication();
			app.UseAuthorization();
			app.UseHangfireDashboard("/dashboard", new DashboardOptions
			{
				Authorization = new[] { new AllowAllUsersAuthorizationFilter() }
			});
			app.MapControllers();

			app.Run();
		}
	}
}
