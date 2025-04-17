using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Epic_Bid.Infrastructure.Persistence._Identity.Migrations
{
    /// <inheritdoc />
    public partial class ForgetPasswordMigrationResetcodeExpireinApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResetCodeExpire",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResetCodeExpire",
                table: "AspNetUsers");
        }
    }
}
