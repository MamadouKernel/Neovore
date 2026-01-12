using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neovore.Web.Migrations
{
    /// <inheritdoc />
    public partial class ActiverCommentairesParDefaut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Activer les commentaires pour tous les services existants
            migrationBuilder.Sql("UPDATE \"Services\" SET \"CommentairesActives\" = true WHERE \"CommentairesActives\" = false;");

            // Activer les commentaires pour tous les produits existants
            migrationBuilder.Sql("UPDATE \"Produits\" SET \"CommentairesActives\" = true WHERE \"CommentairesActives\" = false;");

            // Activer les commentaires pour toutes les réalisations existantes
            migrationBuilder.Sql("UPDATE \"Realisations\" SET \"CommentairesActives\" = true WHERE \"CommentairesActives\" = false;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Pas de rollback nécessaire - on laisse les commentaires activés
        }
    }
}
