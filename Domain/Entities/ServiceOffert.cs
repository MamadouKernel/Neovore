namespace Neovore.Web.Domain.Entities;

public class ServiceOffert : BaseEntity
{
    public string Titre { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Resume { get; set; } = "";
    public string ContenuHtml { get; set; } = "";
    public string? ImageUrl { get; set; }
    public bool EstPublie { get; set; } = true;
    public int Ordre { get; set; } = 0;
    public bool CommentairesActives { get; set; } = true;
    
    public List<Commentaire> Commentaires { get; set; } = new();
}
