using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neovore.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoSupportToMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Medias",
                type: "text",
                nullable: false,
                defaultValue: "image");

            migrationBuilder.AddColumn<string>(
                name: "SousTitresUrl",
                table: "Medias",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionVideo",
                table: "Medias",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "SousTitresUrl",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "DescriptionVideo",
                table: "Medias");
        }
    }
}

