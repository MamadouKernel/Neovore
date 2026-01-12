namespace Neovore.Web.Domain.Entities;

public class Media : BaseEntity
{
    public string Url { get; set; } = "";
    public string? Alt { get; set; }
    public int Ordre { get; set; } = 0;
    
    // Type de média : "image" ou "video"
    public string Type { get; set; } = "image";
    
    // Pour les vidéos : URL du fichier de sous-titres (.vtt)
    public string? SousTitresUrl { get; set; }
    
    // Pour les vidéos : Description textuelle accessible (pour lecteurs d'écran)
    public string? DescriptionVideo { get; set; }

    // Lien optionnel vers produit/réalisation (V1 simple)
    public int? ProduitId { get; set; }
    public Produit? Produit { get; set; }

    public int? RealisationId { get; set; }
    public Realisation? Realisation { get; set; }
}
