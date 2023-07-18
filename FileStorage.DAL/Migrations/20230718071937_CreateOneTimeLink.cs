using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FileStorage.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateOneTimeLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OneTimeLinks",
                columns: table => new
                {
                    LinkId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uri = table.Column<string>(type: "text", nullable: false),
                    FileModelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneTimeLinks", x => x.LinkId);
                    table.UniqueConstraint("AK_OneTimeLinks_Uri", x => x.Uri);
                    table.ForeignKey(
                        name: "FK_OneTimeLinks_Files_FileModelId",
                        column: x => x.FileModelId,
                        principalTable: "Files",
                        principalColumn: "FileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OneTimeLinks_FileModelId",
                table: "OneTimeLinks",
                column: "FileModelId");

            migrationBuilder.CreateIndex(
                name: "IX_OneTimeLinks_LinkId",
                table: "OneTimeLinks",
                column: "LinkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OneTimeLinks");
        }
    }
}
