namespace Neovore.Web.Domain.Entities;

public class AdminUser : BaseEntity
{
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Role { get; set; } = "Admin"; // Admin / Editeur
    public bool IsActive { get; set; } = true;
}
