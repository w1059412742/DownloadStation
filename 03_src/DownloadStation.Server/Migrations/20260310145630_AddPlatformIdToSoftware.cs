using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DownloadStation.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddPlatformIdToSoftware : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoftwarePlatforms");

            migrationBuilder.AddColumn<string>(
                name: "PlatformId",
                table: "Softwares",
                type: "TEXT",
                maxLength: 16,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_PlatformId",
                table: "Softwares",
                column: "PlatformId");

            migrationBuilder.AddForeignKey(
                name: "FK_Softwares_Platforms_PlatformId",
                table: "Softwares",
                column: "PlatformId",
                principalTable: "Platforms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Softwares_Platforms_PlatformId",
                table: "Softwares");

            migrationBuilder.DropIndex(
                name: "IX_Softwares_PlatformId",
                table: "Softwares");

            migrationBuilder.DropColumn(
                name: "PlatformId",
                table: "Softwares");

            migrationBuilder.CreateTable(
                name: "SoftwarePlatforms",
                columns: table => new
                {
                    SoftwareId = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    PlatformId = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwarePlatforms", x => new { x.SoftwareId, x.PlatformId });
                    table.ForeignKey(
                        name: "FK_SoftwarePlatforms_Platforms_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoftwarePlatforms_Softwares_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Softwares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoftwarePlatforms_PlatformId",
                table: "SoftwarePlatforms",
                column: "PlatformId");
        }
    }
}
