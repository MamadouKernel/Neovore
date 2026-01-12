namespace Neovore.Web.Domain.Entities;

public class Realisation : BaseEntity
{
    public string Titre { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Secteur { get; set; } = "";
    public string Description { get; set; } = "";
    public bool EstPublie { get; set; } = true;

    public List<Media> Medias { get; set; } = new();
    public bool CommentairesActives { get; set; } = true;
    
    public List<Commentaire> Commentaires { get; set; } = new();
}
