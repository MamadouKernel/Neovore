using Neovore.Web.Domain.Entities;

namespace Neovore.Web.ViewModels;

public class CommentairePartialVm
{
    public int? ServiceOffertId { get; set; }
    public int? ProduitId { get; set; }
    public int? RealisationId { get; set; }
    public bool CommentairesActives { get; set; } = true;
    public List<Commentaire>? Commentaires { get; set; }
}

