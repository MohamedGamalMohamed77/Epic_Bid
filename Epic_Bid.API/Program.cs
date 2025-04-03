
using Epic_Bid.API.Middlewares;
using Epic_Bid.Apis.Controllers;
using Epic_Bid.Apis.Controllers.Errors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace Epic_Bid.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllers().AddApplicationPart(typeof(AssemblyInformation).Assembly);

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actioncontext) =>
				{
					var errors = actioncontext.ModelState
									.Where(e => e.Value.Errors.Count > 0)
									.SelectMany(p => p.Value.Errors)
									.Select(E => E.ErrorMessage)
									.ToArray();

					var validationerrormessage = new ApiValidationErrorResponse()
					{
						Errors = errors
					};
					return new BadRequestObjectResult(validationerrormessage);

				};
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseMiddleware<ExceptionMiddleWare>();
				app.UseSwagger();
				app.UseSwaggerUI();
			}
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
