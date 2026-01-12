using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neovore.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddLogoUrlToSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "SiteSettings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "SiteSettings");
        }
    }
}
