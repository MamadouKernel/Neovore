using System.ComponentModel.DataAnnotations;

namespace Neovore.Web.Areas.Admin.ViewModels;

public class AdminUserVm
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom d'utilisateur est requis")]
    [Display(Name = "Nom d'utilisateur")]
    public string Username { get; set; } = "";

    [Display(Name = "Rôle")]
    public string Role { get; set; } = "Editeur";

    [Display(Name = "Actif")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Nouveau mot de passe")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
    public string? NewPassword { get; set; }
}

