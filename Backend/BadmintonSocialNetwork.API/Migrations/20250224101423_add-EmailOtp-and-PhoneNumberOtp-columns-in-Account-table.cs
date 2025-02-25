using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSocialNetwork.API.Migrations
{
    /// <inheritdoc />
    public partial class addEmailOtpandPhoneNumberOtpcolumnsinAccounttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Accounts",
                type: "varchar(150)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EmailOtp",
                table: "Accounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumberOtp",
                table: "Accounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "EmailOtp",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PhoneNumberOtp",
                table: "Accounts");
        }
    }
}
