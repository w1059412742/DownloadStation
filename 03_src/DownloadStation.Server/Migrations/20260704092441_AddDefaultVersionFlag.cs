using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DownloadStation.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultVersionFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsDefault",
                table: "SoftwareVersions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
                UPDATE SoftwareVersions
                SET IsDefault = 1
                WHERE Id IN (
                    SELECT Id
                    FROM (
                        SELECT Id,
                               ROW_NUMBER() OVER (PARTITION BY SoftwareId ORDER BY CreatedAt DESC) AS RowNumber
                        FROM SoftwareVersions
                        WHERE IsVisible = 1
                    ) RankedVersions
                    WHERE RowNumber = 1
                );");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareVersions_SoftwareId",
                table: "SoftwareVersions",
                column: "SoftwareId",
                unique: true,
                filter: "\"IsDefault\" = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SoftwareVersions_SoftwareId",
                table: "SoftwareVersions");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "SoftwareVersions");
        }
    }
}
