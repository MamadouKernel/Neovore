using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neovore.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCouleursToSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CouleurAccent",
                table: "SiteSettings",
                type: "text",
                nullable: false,
                defaultValue: "#1E88E5");

            migrationBuilder.AddColumn<string>(
                name: "CouleurOr",
                table: "SiteSettings",
                type: "text",
                nullable: false,
                defaultValue: "#dbb438");

            migrationBuilder.AddColumn<string>(
                name: "CouleurPrimaire",
                table: "SiteSettings",
                type: "text",
                nullable: false,
                defaultValue: "#FF6B35");

            migrationBuilder.AddColumn<string>(
                name: "CouleurSecondaire",
                table: "SiteSettings",
                type: "text",
                nullable: false,
                defaultValue: "#E63946");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouleurAccent",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "CouleurOr",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "CouleurPrimaire",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "CouleurSecondaire",
                table: "SiteSettings");
        }
    }
}

