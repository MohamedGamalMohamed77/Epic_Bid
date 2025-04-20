namespace Epic_Bid.Core.Application.Abstraction.Models.Role
{
    public class UserWithRoleDto
    {
        public required string Id { get; set; }
        public string? UserName { get; set; }

        public string? Email { get; set; }
        public required IEnumerable<string> Roles { get; set; }
    }

}
