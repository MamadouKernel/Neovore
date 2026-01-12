using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neovore.Web.Application.Services;
using Neovore.Web.Infrastructure.Data;
using System.Xml.Linq;

namespace Neovore.Web.Controllers;

public class SitemapController : Controller
{
    private readonly AppDbContext _db;
    private readonly ServicesService _servicesService;
    private readonly CatalogueService _catalogueService;
    private readonly RealisationsService _realisationsService;

    public SitemapController(
        AppDbContext db,
        ServicesService servicesService,
        CatalogueService catalogueService,
        RealisationsService realisationsService)
    {
        _db = db;
        _servicesService = servicesService;
        _catalogueService = catalogueService;
        _realisationsService = realisationsService;
    }

    [HttpGet("/sitemap.xml")]
    public async Task<IActionResult> Index()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var sitemap = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("urlset",
                new XAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9"),
                
                // Page d'accueil
                new XElement("url",
                    new XElement("loc", baseUrl),
                    new XElement("changefreq", "weekly"),
                    new XElement("priority", "1.0")
                ),
                
                // Pages statiques
                new XElement("url",
                    new XElement("loc", $"{baseUrl}/Services"),
                    new XElement("changefreq", "weekly"),
                    new XElement("priority", "0.9")
                ),
                new XElement("url",
                    new XElement("loc", $"{baseUrl}/Produits"),
                    new XElement("changefreq", "weekly"),
                    new XElement("priority", "0.9")
                ),
                new XElement("url",
                    new XElement("loc", $"{baseUrl}/Realisations"),
                    new XElement("changefreq", "weekly"),
                    new XElement("priority", "0.9")
                ),
                new XElement("url",
                    new XElement("loc", $"{baseUrl}/devis"),
                    new XElement("changefreq", "monthly"),
                    new XElement("priority", "0.8")
                ),
                new XElement("url",
                    new XElement("loc", $"{baseUrl}/contact"),
                    new XElement("changefreq", "monthly"),
                    new XElement("priority", "0.8")
                ),
                
                // Services
                (await _servicesService.GetPubliesAsync()).Select(s => new XElement("url",
                    new XElement("loc", $"{baseUrl}/services/{s.Slug}"),
                    new XElement("changefreq", "monthly"),
                    new XElement("priority", "0.7")
                )),
                
                // Produits
                (await _catalogueService.GetProduitsAsync(null)).Select(p => new XElement("url",
                    new XElement("loc", $"{baseUrl}/produits/{p.Slug}"),
                    new XElement("changefreq", "monthly"),
                    new XElement("priority", "0.7")
                )),
                
                // Réalisations
                (await _realisationsService.GetPubliesAsync()).Select(r => new XElement("url",
                    new XElement("loc", $"{baseUrl}/realisations/{r.Slug}"),
                    new XElement("changefreq", "monthly"),
                    new XElement("priority", "0.7")
                ))
            )
        );

        return Content(sitemap.ToString(), "application/xml", System.Text.Encoding.UTF8);
    }
}

