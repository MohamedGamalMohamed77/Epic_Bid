using Epic_Bid.Core.Application.Abstraction.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic_Bid.Core.Application.Abstraction.Services.Role
{
    public interface IUserService
    {
        Task<IReadOnlyList<UserWithRoleDto>> GetUsersWithRolesAsync();
        Task<UserWithRoleDto> EditUserRoles(string userId, List<string> RolesNames);

    }
}
