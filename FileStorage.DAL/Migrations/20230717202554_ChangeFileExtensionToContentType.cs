using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileStorage.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFileExtensionToContentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileExtension",
                table: "Files",
                newName: "ContentType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "Files",
                newName: "FileExtension");
        }
    }
}
