namespace Neovore.Web.Domain.Entities;

public class ContactMessage : BaseEntity
{
    public string Nom { get; set; } = "";
    public string Email { get; set; } = "";
    public string Telephone { get; set; } = "";
    public string Sujet { get; set; } = "";
    public string Message { get; set; } = "";
    public string Statut { get; set; } = "Nouveau";
}
