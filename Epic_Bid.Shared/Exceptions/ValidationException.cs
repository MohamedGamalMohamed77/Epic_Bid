﻿using Epic_Bid.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Shared.Exceptions
{
	public class ValidationException: BadRequestException
	{
		public required IEnumerable<string> Errors { get; set; }
		public ValidationException(string? message = "Bad Request")
			: base(message)
		{

		}

	}
}
