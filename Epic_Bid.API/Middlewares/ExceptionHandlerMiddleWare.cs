using Epic_Bid.Apis.Controllers.Controllers.Errors;
using Epic_Bid.Core.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace Epic_Bid.API.Middlewares
{
	public class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlerMiddleware> _logger;
		private readonly IWebHostEnvironment _environment;

		public ExceptionHandlerMiddleware(RequestDelegate next,
			ILogger<ExceptionHandlerMiddleware> logger, IWebHostEnvironment environment)
		{
			_next = next;
			_logger = logger;
			_environment = environment;
		}
		//public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
		//{
		//	return builder.UseMiddleware<ExceptionHandlerMiddleWare>();
		//}
		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);

				//if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)
				//{
				//	var response = new ApiResponse((int)HttpStatusCode.NotFound, $"the requested endpoint : {httpContext.Request.Path}not found");

				//	 await httpContext.Response.WriteAsync(response.ToString());
				//}
			}
			catch (Exception ex)
			{

				#region Logging :TODO
				if (_environment.IsDevelopment())
				{
					_logger.LogError(ex, ex.Message);

				}
				else
				{
				}
				#endregion

				await HandleExceptions(httpContext, ex);
			}

		}

		private async Task HandleExceptions(HttpContext httpContext, Exception ex)
		{
			ApiResponse response;
			switch (ex)
			{
				case NotFoundException:
					httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
					httpContext.Response.ContentType = "application/json";
					response = new ApiResponse(404, ex.Message);

					await httpContext.Response.WriteAsync(response.ToString()!);
					break;

				case ValidationException validationException:
					httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
					httpContext.Response.ContentType = "application/json";

					response = new ApiValidationErrorResponse(ex.Message) { Errors = (IEnumerable<ApiValidationErrorResponse.ValidationError>)validationException.Errors };

					await httpContext.Response.WriteAsync(response.ToString()!);
					break;

				case BadRequestException:
					httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
					httpContext.Response.ContentType = "application/json";
					response = new ApiResponse(400, ex.Message);
					await httpContext.Response.WriteAsync(response.ToString()!);
					break;
				case UnAuthorizedException:
					httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					httpContext.Response.ContentType = "application/json";
					response = new ApiResponse(401, ex.Message);
					await httpContext.Response.WriteAsync(response.ToString()!);
					break;

				default:
					response = _environment.IsDevelopment() ?
							new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace?.ToString())
							:
							new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

					httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					httpContext.Response.ContentType = "application/json";
					await httpContext.Response.WriteAsync(response.ToString()!);
					break;
			}
		}
	}
}

