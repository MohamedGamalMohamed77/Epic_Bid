using Epic_Bid.Apis.Controllers.Errors;
using System.Net;
using System.Text.Json;

namespace Epic_Bid.API.Middlewares
{
    public class ExceptionMiddleWare
    {
        public RequestDelegate _Next { get; }
        public ILogger<ExceptionMiddleWare> _Logger { get; }
        public IHostEnvironment _Env { get; }
        public ExceptionMiddleWare(RequestDelegate Next, ILogger<ExceptionMiddleWare> Logger, IHostEnvironment Env)
        {
            this._Next = Next;
            this._Logger = Logger;
            this._Env = Env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _Next.Invoke(context);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                if (_Env.IsDevelopment())
                {
                    var Response = new ApiExceptionResponse(500, ex.Message, ex.StackTrace.ToString());
                    var option = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

                    };
                    var jsonResponse = JsonSerializer.Serialize(Response, option);
                    await context.Response.WriteAsync(jsonResponse);
                }
                else
                {
                    var Response = new ApiExceptionResponse(500);
                    var option = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    var jsonResponse = JsonSerializer.Serialize(Response,option);
                    await context.Response.WriteAsync(jsonResponse);
                }

            }
        }
        

       
    }
}
