using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neovore.Web.Infrastructure.Data;

namespace Neovore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "EditorOrAdmin")]
public class DemandesController : Controller
{
    private readonly AppDbContext _db;

    public DemandesController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Devis()
        => View(await _db.DemandesDevis.OrderByDescending(x => x.CreatedAtUtc).ToListAsync());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> MajStatutDevis(int id, string statut)
    {
        var d = await _db.DemandesDevis.FindAsync(id);
        if (d is null) return NotFound();
        d.Statut = statut;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Devis));
    }

    public async Task<IActionResult> Contacts()
        => View(await _db.ContactMessages.OrderByDescending(x => x.CreatedAtUtc).ToListAsync());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> MajStatutContact(int id, string statut)
    {
        var m = await _db.ContactMessages.FindAsync(id);
        if (m is null) return NotFound();
        m.Statut = statut;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Contacts));
    }
}
