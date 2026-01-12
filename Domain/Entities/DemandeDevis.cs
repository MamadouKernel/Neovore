namespace Neovore.Web.Domain.Entities;

public class DemandeDevis : BaseEntity
{
    public string Nom { get; set; } = "";
    public string Societe { get; set; } = "";
    public string Telephone { get; set; } = "";
    public string Email { get; set; } = "";
    public string Secteur { get; set; } = "";
    public string TypeBesoin { get; set; } = "";
    public string Message { get; set; } = "";
    public string Statut { get; set; } = "Nouveau"; // Nouveau / EnCours / Traite
}
