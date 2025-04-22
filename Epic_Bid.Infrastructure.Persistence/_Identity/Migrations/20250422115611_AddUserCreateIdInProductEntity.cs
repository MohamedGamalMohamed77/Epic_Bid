using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Epic_Bid.Infrastructure.Persistence._Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddUserCreateIdInProductEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserCreatedId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserCreatedId",
                table: "Products");
        }
    }
}
