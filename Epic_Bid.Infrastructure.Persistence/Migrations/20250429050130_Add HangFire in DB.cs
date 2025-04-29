using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Epic_Bid.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddHangFireinDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuctionCloseJobId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuctionEmailJobId",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuctionCloseJobId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AuctionEmailJobId",
                table: "Products");
        }
    }
}
