using Epic_Bid.Core.Application.Abstraction.Models.Role;
using Epic_Bid.Core.Application.Abstraction.Services.Role;
using Epic_Bid.Shared.Exceptions;
using Epic_Bid.Core.Domain.Entities.Auth;
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
    public class UserService( UserManager<ApplicationUser> _userManager) : IUserService
    {
        public async Task<UserWithRoleDto> EditUserRoles(string userId, List<string> RolesNames)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new BadRequestException("User not found");

            if (RolesNames is null || RolesNames.Count == 0)
                throw new BadRequestException("You must enter at least one role");

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var result = await _userManager.AddToRolesAsync(user, RolesNames);
            if (!result.Succeeded)
                throw new ValidationException { Errors = result.Errors.Select(e => e.Description).ToList() };

            return new UserWithRoleDto
            {
                Id = user.Id,
                UserName = user.DisplayName,
                Email = user.Email!,
                Roles = await _userManager.GetRolesAsync(user)
            };
        }

        public async Task<IReadOnlyList<UserWithRoleDto>> GetUsersWithRolesAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userWithRolesList = new List<UserWithRoleDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userWithRolesList.Add(new UserWithRoleDto
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    Roles = roles
                });
            }

            if (!userWithRolesList.Any())
                throw new BadRequestException("No users found");

            return userWithRolesList;
        }
    }
}
