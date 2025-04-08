﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Exceptions
{
	public class UnAuthorizedException : ApplicationException
	{
		public UnAuthorizedException(string message) : base(message)
		{

		}

	}
}
