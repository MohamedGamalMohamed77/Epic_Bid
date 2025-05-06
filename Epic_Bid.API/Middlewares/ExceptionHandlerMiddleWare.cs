using Epic_Bid.Apis.Controllers.Controllers.Errors;
using Epic_Bid.Shared.Exceptions;
using System.Diagnostics;
using System.Net;

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
                await _next.Invoke(httpContext);

                // EndpointNotFound
				await NotFoundEndPointHandler(httpContext);
            }
            catch (Exception ex)
			{

				#region Logging :TODO
				//if (_environment.IsDevelopment())
				//{
				//	_logger.LogError(ex, ex.Message);

				//}
				//else
				//{
				//}
				#endregion

				await HandleExceptions(httpContext, ex);
			}

		}

        private static async Task NotFoundEndPointHandler(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var respone = new ApiResponse(StatusCodes.Status404NotFound, $"Endpoint {httpContext.Request.Path} Not Found");
                await httpContext.Response.WriteAsJsonAsync(respone);
            }
			if(httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
			{
                var respone = new ApiResponse(StatusCodes.Status401Unauthorized, $"Not Unauthorized");
                await httpContext.Response.WriteAsJsonAsync(respone);
            }
		

        }
		

        private async Task HandleExceptions(HttpContext httpContext, Exception ex)
		{
			ApiResponse response;
			switch (ex)
			{
				
				case NotFoundException:
					httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
					await httpContext.Response.WriteAsJsonAsync(new ApiResponse(404, ex.Message));
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
					response = new ApiExceptionResponse(StatusCodes.Status500InternalServerError, ex.Message, ex.StackTrace?.ToString());
					httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
					httpContext.Response.ContentType = "application/json";
					await httpContext.Response.WriteAsync(response.ToString()!);
					//httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					//response = new ApiExceptionResponse(500, ex.Message, ex.StackTrace?.ToString());
					//await httpContext.Response.WriteAsJsonAsync(response.ToString());
					break;
			}
		}
	}
}

