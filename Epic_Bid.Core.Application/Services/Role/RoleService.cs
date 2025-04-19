using Epic_Bid.Core.Application.Abstraction.Services.Role;
using Epic_Bid.Core.Application.Exceptions;
using Epic_Bid.Core.Domain.Entities.Roles;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Services.Role
{
    public class RoleService(RoleManager<AppRole> _RoleManager) : IRoleService
    {
        public async Task CreateRoleAsync(string roleName)
        {
            var Role = new AppRole()
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            var result = _RoleManager.CreateAsync(Role);
            if (!result.Result.Succeeded)
            
            {
                throw new BadRequestException("Role Not Created");
            }
        }

        public async Task DeleteRoleAsync(string roleName)
        {
            var Role = await _RoleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (Role is null)
            {
                throw new BadRequestException("Role Not Found");
            }
            var result = await _RoleManager.DeleteAsync(Role);
            if (!result.Succeeded)
            
            {
                throw new BadRequestException("Role Not Deleted");
            }

        }

        public async Task<List<AppRole>> GetAllRoles()
        {
            var Roles = await _RoleManager.Roles.ToListAsync();
            if(Roles.Count == 0)
            {
                throw new BadRequestException("No Roles Found");
            }
            return Roles;
        }

        public async Task<AppRole> GetRoleIdAsync(string roleName)
        {
            var Role = await _RoleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (Role is null)
            {
                throw new BadRequestException("Role Not Found");
            }
            return Role;
        }

        public async Task<bool> RoleExists(string roleName)
        {
            var Role  = await _RoleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (Role is null)
            {
                return false;
            }
            return true;
        }

        public async Task UpdateRoleAsync(string roleName, string newRoleName)
        {
            var Role = await _RoleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (Role is null)
            {
                throw new BadRequestException("Role Not Found");
            }
            Role.Name = newRoleName;
            Role.NormalizedName = newRoleName.ToUpper();
            Role.ConcurrencyStamp = Guid.NewGuid().ToString();
            var result = await _RoleManager.UpdateAsync(Role);
            if (!result.Succeeded)
           
            {
                throw new BadRequestException("Role Not Updated");
            }
        }
    }

}
