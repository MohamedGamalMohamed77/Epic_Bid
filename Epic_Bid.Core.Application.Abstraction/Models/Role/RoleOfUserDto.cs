namespace Epic_Bid.Core.Application.Abstraction.Models.Role
{
	public class RoleOfUserDto
	{
		public required string UserId { get; set; }
		public List<string> RolesNames { get; set; } = new List<string>();
	}
}
