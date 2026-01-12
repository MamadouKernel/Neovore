using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neovore.Web.Application.Services;

namespace Neovore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = "EditorOrAdmin")]
public class AnalyticsController : Controller
{
    private readonly AnalyticsService _analyticsService;

    public AnalyticsController(AnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    public async Task<IActionResult> Index(DateTime? dateDebut, DateTime? dateFin)
    {
        var stats = await _analyticsService.GetStatsAsync(dateDebut, dateFin);
        ViewBag.DateDebut = dateDebut;
        ViewBag.DateFin = dateFin;
        return View(stats);
    }
}


