﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Epic_Bid.Apis.Controllers.Controllers.Errors;

public class ApiResponse
{
	public int? StatusCode { get; set; }
	public string? Message { get; set; }
	public ApiResponse(int? statuscode, string? message = null)
	{
		StatusCode = statuscode;
		Message = message ?? GetDefaultMessageForStatusCode(statuscode);
	}

	private string? GetDefaultMessageForStatusCode(int? statuscode)
	{
		return statuscode switch
		{
			400 => "A bad request, you have made",
			401 => "Authorized, you are not",
			404 => "Resource found, it was not",
			500 => "Internal server error",
			_ => null
		};
	}


		public override string ToString() => JsonSerializer.Serialize(this, new JsonSerializerOptions()
		{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

}

