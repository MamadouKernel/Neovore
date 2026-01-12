namespace Neovore.Web.Domain.Entities;

public class Produit : BaseEntity
{
    public string Nom { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Description { get; set; } = "";
    public string Specifications { get; set; } = ""; // texte simple (V1)
    public decimal? Prix { get; set; }
    public bool EstPublie { get; set; } = true;

    public int CategorieProduitId { get; set; }
    public CategorieProduit? Categorie { get; set; }

    public List<Media> Medias { get; set; } = new();
    public bool CommentairesActives { get; set; } = true;
    
    public List<Commentaire> Commentaires { get; set; } = new();
}
