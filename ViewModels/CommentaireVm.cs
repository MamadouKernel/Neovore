using System.ComponentModel.DataAnnotations;

namespace Neovore.Web.ViewModels;

public class CommentaireVm
{
    [Required(ErrorMessage = "Le nom est requis")]
    [Display(Name = "Nom complet")]
    public string Nom { get; set; } = "";

    [Required(ErrorMessage = "L'email est requis")]
    [EmailAddress(ErrorMessage = "Email invalide")]
    [Display(Name = "Email")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Le message est requis")]
    [Display(Name = "Commentaire")]
    [StringLength(1000, ErrorMessage = "Le commentaire ne peut pas dépasser 1000 caractères")]
    public string Message { get; set; } = "";

    [Range(1, 5, ErrorMessage = "La note doit être entre 1 et 5")]
    [Display(Name = "Note (optionnelle)")]
    public int? Note { get; set; }

    // Identifiant de l'entité commentée
    public int? ServiceOffertId { get; set; }
    public int? ProduitId { get; set; }
    public int? RealisationId { get; set; }

    // Anti-spam
    public string? Website { get; set; }
}

