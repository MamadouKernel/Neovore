using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neovore.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddAboutPageContentToSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TexteHistoire",
                table: "SiteSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TexteValeurEngagement",
                table: "SiteSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TexteValeurExcellence",
                table: "SiteSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TexteValeurFiabilite",
                table: "SiteSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TexteValeurInnovation",
                table: "SiteSettings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TexteHistoire",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "TexteValeurEngagement",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "TexteValeurExcellence",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "TexteValeurFiabilite",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "TexteValeurInnovation",
                table: "SiteSettings");
        }
    }
}
