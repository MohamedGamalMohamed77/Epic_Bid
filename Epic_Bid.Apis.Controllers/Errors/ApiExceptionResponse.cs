using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Apis.Controllers.Errors
{
    public class ApiExceptionResponse:ApiResponse
    {
        public string? Details { get; set; }
        public ApiExceptionResponse(int statuscode,string? message = null ,string? details = null):base(statuscode, message)
        {
            Details = details;
        }
    }
}
