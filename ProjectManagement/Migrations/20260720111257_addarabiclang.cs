using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagement.Migrations
{
    /// <inheritdoc />
    public partial class addarabiclang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skill_AspNetUsers_UserId",
                table: "Skill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skill",
                table: "Skill");

            migrationBuilder.RenameTable(
                name: "Skill",
                newName: "Skills");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Experiences",
                newName: "TitleEn");

            migrationBuilder.RenameColumn(
                name: "Company",
                table: "Experiences",
                newName: "TitleAr");

            migrationBuilder.RenameColumn(
                name: "Field",
                table: "Educations",
                newName: "FieldEn");

            migrationBuilder.RenameColumn(
                name: "Degree",
                table: "Educations",
                newName: "FieldAr");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "AspNetUsers",
                newName: "TitleEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "TitleAr");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "AspNetUsers",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Company",
                table: "AspNetUsers",
                newName: "NameAr");

            migrationBuilder.RenameColumn(
                name: "About",
                table: "AspNetUsers",
                newName: "LocationEn");

            migrationBuilder.RenameIndex(
                name: "IX_Skill_UserId",
                table: "Skills",
                newName: "IX_Skills_UserId");

            migrationBuilder.AddColumn<string>(
                name: "CompanyAr",
                table: "Experiences",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEn",
                table: "Experiences",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DegreeAr",
                table: "Educations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DegreeEn",
                table: "Educations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AboutAr",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AboutEn",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyAr",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEn",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationAr",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skills",
                table: "Skills",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_AspNetUsers_UserId",
                table: "Skills",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_AspNetUsers_UserId",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skills",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CompanyAr",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "CompanyEn",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "DegreeAr",
                table: "Educations");

            migrationBuilder.DropColumn(
                name: "DegreeEn",
                table: "Educations");

            migrationBuilder.DropColumn(
                name: "AboutAr",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AboutEn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyAr",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyEn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LocationAr",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Skills",
                newName: "Skill");

            migrationBuilder.RenameColumn(
                name: "TitleEn",
                table: "Experiences",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "TitleAr",
                table: "Experiences",
                newName: "Company");

            migrationBuilder.RenameColumn(
                name: "FieldEn",
                table: "Educations",
                newName: "Field");

            migrationBuilder.RenameColumn(
                name: "FieldAr",
                table: "Educations",
                newName: "Degree");

            migrationBuilder.RenameColumn(
                name: "TitleEn",
                table: "AspNetUsers",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "TitleAr",
                table: "AspNetUsers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "AspNetUsers",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "NameAr",
                table: "AspNetUsers",
                newName: "Company");

            migrationBuilder.RenameColumn(
                name: "LocationEn",
                table: "AspNetUsers",
                newName: "About");

            migrationBuilder.RenameIndex(
                name: "IX_Skills_UserId",
                table: "Skill",
                newName: "IX_Skill_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skill",
                table: "Skill",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_AspNetUsers_UserId",
                table: "Skill",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
