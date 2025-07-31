using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedResumeStringField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_ApplicantId",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "IsSeeker",
                table: "Users",
                newName: "IsHirer");

            migrationBuilder.RenameColumn(
                name: "ApplicantId",
                table: "Applications",
                newName: "SeekerId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_ApplicantId",
                table: "Applications",
                newName: "IX_Applications_SeekerId");

            migrationBuilder.AddColumn<string>(
                name: "ResumeJsonString",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AIGivenRating",
                table: "Applications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_SeekerId",
                table: "Applications",
                column: "SeekerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_SeekerId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ResumeJsonString",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AIGivenRating",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "IsHirer",
                table: "Users",
                newName: "IsSeeker");

            migrationBuilder.RenameColumn(
                name: "SeekerId",
                table: "Applications",
                newName: "ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_SeekerId",
                table: "Applications",
                newName: "IX_Applications_ApplicantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_ApplicantId",
                table: "Applications",
                column: "ApplicantId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
