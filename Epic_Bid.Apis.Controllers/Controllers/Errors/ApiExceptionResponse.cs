using System.Text.Json;

namespace Epic_Bid.Apis.Controllers.Controllers.Errors;

public class ApiExceptionResponse:ApiResponse
{
    public string? Details { get; set; }
    public ApiExceptionResponse(int statuscode,string? message = null ,string? details = null):base(statuscode, message)
    {
        Details = details;
    }
	public override string ToString()
			=> JsonSerializer.Serialize(this, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

}
