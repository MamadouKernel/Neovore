using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neovore.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddSocialMediaNetworks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rendre FacebookUrl et LinkedInUrl nullable (déjà existants)
            migrationBuilder.AlterColumn<string>(
                name: "FacebookUrl",
                table: "SiteSettings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: false,
                oldDefaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "LinkedInUrl",
                table: "SiteSettings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: false,
                oldDefaultValue: "");

            // Ajouter les nouveaux réseaux sociaux
            migrationBuilder.AddColumn<string>(
                name: "TwitterUrl",
                table: "SiteSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramUrl",
                table: "SiteSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YouTubeUrl",
                table: "SiteSettings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TikTokUrl",
                table: "SiteSettings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Supprimer les nouveaux réseaux sociaux
            migrationBuilder.DropColumn(
                name: "TwitterUrl",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "InstagramUrl",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "YouTubeUrl",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "TikTokUrl",
                table: "SiteSettings");

            // Remettre FacebookUrl et LinkedInUrl comme non-nullable
            migrationBuilder.AlterColumn<string>(
                name: "FacebookUrl",
                table: "SiteSettings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LinkedInUrl",
                table: "SiteSettings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}

