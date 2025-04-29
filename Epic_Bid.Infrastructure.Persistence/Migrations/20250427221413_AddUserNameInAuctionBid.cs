using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Epic_Bid.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNameInAuctionBid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "AuctionBids",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "AuctionBids");
        }
    }
}
