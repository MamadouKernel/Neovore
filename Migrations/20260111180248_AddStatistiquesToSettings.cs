using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neovore.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddStatistiquesToSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnneesExperience",
                table: "SiteSettings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjetsRealises",
                table: "SiteSettings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SatisfactionClient",
                table: "SiteSettings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnneesExperience",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "ProjetsRealises",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "SatisfactionClient",
                table: "SiteSettings");
        }
    }
}
