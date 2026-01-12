namespace Neovore.Web.Domain.Entities;

public class CategorieProduit : BaseEntity
{
    public string Nom { get; set; } = "";
    public string Slug { get; set; } = "";
    public int Ordre { get; set; } = 0;
    public bool EstActive { get; set; } = true;

    public List<Produit> Produits { get; set; } = new();
}
