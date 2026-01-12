namespace Neovore.Web.Domain.Entities;

public class Commentaire : BaseEntity
{
    public string Nom { get; set; } = "";
    public string Email { get; set; } = "";
    public string Message { get; set; } = "";
    public int? Note { get; set; } // 1-5 étoiles
    public bool EstApprouve { get; set; } = false;
    public bool EstRejete { get; set; } = false;
    
    // Relations (une seule sera remplie)
    public int? ServiceOffertId { get; set; }
    public ServiceOffert? ServiceOffert { get; set; }
    
    public int? ProduitId { get; set; }
    public Produit? Produit { get; set; }
    
    public int? RealisationId { get; set; }
    public Realisation? Realisation { get; set; }
}

