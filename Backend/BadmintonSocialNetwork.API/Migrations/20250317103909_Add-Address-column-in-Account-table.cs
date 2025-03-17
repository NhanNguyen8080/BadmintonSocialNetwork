using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSocialNetwork.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAddresscolumninAccounttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Accounts");
        }
    }
}
