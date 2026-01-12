namespace Neovore.Web.Domain.Entities;

public class Partenaire : BaseEntity
{
    public string Nom { get; set; } = "";
    public string? LogoUrl { get; set; }
    public string? SiteWeb { get; set; }
    public string? Description { get; set; }
    public bool EstPublie { get; set; } = false;
    public int Ordre { get; set; } = 0;
    public string TypePartenaire { get; set; } = "Client"; // Client, Partenaire, Fournisseur
}

