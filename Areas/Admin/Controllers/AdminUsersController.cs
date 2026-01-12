using System.Security.Claims;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neovore.Web.Domain.Entities;
using Neovore.Web.Infrastructure.Repositories;

namespace Neovore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "AdminOnly")]
public class AdminUsersController : Controller
{
    private readonly IGenericRepository<AdminUser> _repo;

    public AdminUsersController(IGenericRepository<AdminUser> repo)
    {
        _repo = repo;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _repo.Query()
            .OrderBy(u => u.Username)
            .ToListAsync();
        return View(users);
    }

    public IActionResult Create()
    {
        return View(new AdminUser { Role = "Editeur", IsActive = true });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AdminUser model, string password)
    {
        if (!ModelState.IsValid) return View(model);

        if (string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError("", "Le mot de passe est requis.");
            return View(model);
        }

        // Vérifier si l'utilisateur existe déjà
        var existing = await _repo.Query().FirstOrDefaultAsync(u => u.Username == model.Username);
        if (existing != null)
        {
            ModelState.AddModelError("", "Ce nom d'utilisateur existe déjà.");
            return View(model);
        }

        model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        await _repo.AddAsync(model);
        await _repo.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user is null) return NotFound();

        // Empêcher la modification de son propre compte (pour éviter les problèmes)
        var currentUsername = User.FindFirstValue(ClaimTypes.Name);
        if (user.Username == currentUsername && User.IsInRole("Admin"))
        {
            // On peut modifier son propre compte, mais avec prudence
        }

        return View(user);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AdminUser model, string? newPassword)
    {
        var existing = await _repo.GetByIdAsync(model.Id);
        if (existing is null) return NotFound();

        // Vérifier si le nom d'utilisateur change et s'il existe déjà
        if (existing.Username != model.Username)
        {
            var usernameExists = await _repo.Query().AnyAsync(u => u.Username == model.Username && u.Id != model.Id);
            if (usernameExists)
            {
                ModelState.AddModelError("", "Ce nom d'utilisateur existe déjà.");
                model.PasswordHash = existing.PasswordHash;
                return View(model);
            }
        }

        if (!ModelState.IsValid)
        {
            model.PasswordHash = existing.PasswordHash;
            return View(model);
        }

        // Mettre à jour les propriétés
        existing.Username = model.Username;
        existing.Role = model.Role;
        existing.IsActive = model.IsActive;
        existing.UpdatedAtUtc = DateTime.UtcNow;

        // Mettre à jour le mot de passe seulement si fourni
        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        }

        await _repo.UpdateAsync(existing);
        await _repo.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user is null) return NotFound();

        // Empêcher la suppression de son propre compte
        var currentUsername = User.FindFirstValue(ClaimTypes.Name);
        if (user.Username == currentUsername)
        {
            TempData["Error"] = "Vous ne pouvez pas supprimer votre propre compte.";
            return RedirectToAction(nameof(Index));
        }

        await _repo.DeleteAsync(user);
        await _repo.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

