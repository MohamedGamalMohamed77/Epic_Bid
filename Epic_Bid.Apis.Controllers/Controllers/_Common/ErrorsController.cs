using Microsoft.AspNetCore.Mvc;
using Epic_Bid.Apis.Controllers.Controllers.Errors;
using System.Net;
namespace Epic_Bid.Apis.Controllers.Controllers._Common
{


	[ApiController]
	[Route("Errors/{Code}")]
	[ApiExplorerSettings(IgnoreApi = false)]
	public class ErrorsController : ControllerBase
	{
		[HttpGet]
		public IActionResult Error(int Code)
		{
			if (Code == (int)HttpStatusCode.NotFound)
			{
				var response = new ApiResponse((int)HttpStatusCode.NotFound, $"the requested endpoint : {Request.Path}not found");
				return NotFound(response);
			}

			return StatusCode(Code, new ApiResponse(Code));

		}
	}
}
