using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neovore.Web.Infrastructure.Data;

namespace Neovore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "EditorOrAdmin")]
public class DashboardController : Controller
{
    private readonly AppDbContext _db;

    public DashboardController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.NbDevis = await _db.DemandesDevis.CountAsync();
        ViewBag.NbMessages = await _db.ContactMessages.CountAsync();
        ViewBag.NbProduits = await _db.Produits.CountAsync();
        ViewBag.NbRealisations = await _db.Realisations.CountAsync();
        return View();
    }
}
