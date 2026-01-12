namespace Neovore.Web.Domain.Entities;

public class MembreEquipe : BaseEntity
{
    public string Nom { get; set; } = "";
    public string Prenom { get; set; } = "";
    public string? Poste { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? Description { get; set; }
    public bool EstPublie { get; set; } = false;
    public int Ordre { get; set; } = 0;
}

