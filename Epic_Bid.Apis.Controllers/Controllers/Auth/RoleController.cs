using Epic_Bid.Apis.Controllers.Controllers.Base;
using Epic_Bid.Apis.Controllers.Controllers.Errors;
using Epic_Bid.Core.Application.Abstraction.Services.Role;
using Epic_Bid.Core.Domain.Entities.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

namespace Epic_Bid.Apis.Controllers.Controllers.Auth
{
    [Authorize(Roles = "Admin")]
    public class RoleController(IRoleService _roleService) : BaseApiController
    {
        [HttpPost("CreateRole")]
        public async Task<ActionResult<string>> CreateRole(string roleName)
        {
            var roleExist = await _roleService.RoleExists(roleName);
            if (roleExist) return BadRequest(new ApiResponse(400, "Role already exists"));
            await _roleService.CreateRoleAsync(roleName);
            return Ok(roleName);
        }

        [HttpGet("GetAllRoles")]
        public async Task<ActionResult<List<AppRole>>> GetAllRoles()
        {
            var roles = await _roleService.GetAllRoles();
            return Ok(roles);
        }

        [HttpPut("UpdateRole")]
        public async Task<ActionResult<string>> UpdateRole(string name, string roleName)
        {
            var role = await _roleService.RoleExists(name);
            if (!role) return BadRequest(new ApiResponse(400, "Role not found"));

            await _roleService.UpdateRoleAsync(name, roleName);
            return Ok(roleName);
        }

        [HttpDelete("DeleteRole")]
        public async Task<ActionResult<bool>> DeleteRole(string name)
        {
            var role = await _roleService.RoleExists(name);
            if (!role) return BadRequest(new ApiResponse(400, "Role not found"));

            await _roleService.DeleteRoleAsync(name);
            return Ok(true);
        }
    }
}
