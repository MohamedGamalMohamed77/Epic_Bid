using Epic_Bid.Apis.Controllers.Controllers.Base;
using Epic_Bid.Apis.Controllers.Controllers.Errors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Apis.Controllers.Controllers.Buggy
{

    public class BuggyController : BaseApiController
    {
        //Nnot found
        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            return NotFound(new ApiResponse(404));
        }
        //Bad request
        [HttpGet("BadRequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        //Validation-Error
        [HttpGet("BadRequest/{id}")]
        public IActionResult GetBadRequest(int id)
        {
            return Ok();
        }
        //Server Error

        [HttpGet("servererror")]
        public IActionResult ThrowError()
        {
            // رمي استثناء متعمد عشان نشوف الـ Middleware بيتعامل معاه ازاي
            throw new Exception("هذا خطأ متعمد لاختبار الـ Middleware!");
        }
        
       
    }
}
