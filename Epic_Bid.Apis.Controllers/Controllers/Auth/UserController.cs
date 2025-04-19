using Epic_Bid.Apis.Controllers.Controllers.Base;
using Epic_Bid.Core.Application.Abstraction.Models.Role;
using Epic_Bid.Core.Application.Abstraction.Services.Role;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

namespace Epic_Bid.Apis.Controllers.Controllers.Auth
{
    [Authorize(Roles = "Admin")]
    public class UserController(IUserService _UserService) : BaseApiController
    {
        [HttpGet("GetAllUserWithRoles")]
        public async Task<ActionResult<IReadOnlyList<UserWithRoleDto>>> GetAllUserWithRoles()
        {
            var result = await _UserService.GetUsersWithRolesAsync();
            return Ok(result);
        }

        [HttpPut("EditUserRoles")]
        public async Task<ActionResult> EditUserRoles(UserWithRoleDto userWithRoleDto)
        {
            if (userWithRoleDto == null) return BadRequest("User with roles is null");
            var result = await _UserService.EditUserRoles(userWithRoleDto.Id, userWithRoleDto.Roles.ToList());
            return Ok(result);
        }
    }
}
