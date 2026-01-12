using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neovore.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddVideos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionVideo",
                table: "Medias",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SousTitresUrl",
                table: "Medias",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Medias",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionVideo",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "SousTitresUrl",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Medias");
        }
    }
}
