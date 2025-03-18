using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BadmintonSocialNetwork.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangedatatypeofParentCommentIdinCommenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentCommentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ParentCommentID",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "ParentCommentId",
                table: "Comments",
                newName: "ParentCommentID");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                newName: "IX_Comments_ParentCommentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentCommentID",
                table: "Comments",
                column: "ParentCommentID",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentCommentID",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "ParentCommentID",
                table: "Comments",
                newName: "ParentCommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ParentCommentID",
                table: "Comments",
                newName: "IX_Comments_ParentCommentId");

            migrationBuilder.AddColumn<int>(
                name: "ParentCommentID",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
