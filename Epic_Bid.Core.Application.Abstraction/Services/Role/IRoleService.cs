using Epic_Bid.Core.Domain.Entities.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Services.Role
{
    public interface IRoleService 
    {
        Task CreateRoleAsync(string roleName);
        Task DeleteRoleAsync(string roleName);
        Task UpdateRoleAsync(string roleName, string newRoleName);
        Task<List<AppRole>> GetAllRoles();
        // exist
        Task<bool> RoleExists(string roleName);

        Task<AppRole> GetRoleIdAsync(string roleName);
    }
}
