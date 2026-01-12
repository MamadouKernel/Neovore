using Neovore.Web.Application.Services;

namespace Neovore.Web.Infrastructure.Middleware;

public class AnalyticsMiddleware
{
    private readonly RequestDelegate _next;

    public AnalyticsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AnalyticsService analyticsService)
    {
        // Ignorer les requêtes admin et les assets
        var path = context.Request.Path.Value ?? "";
        if (path.StartsWith("/Admin", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/css", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/js", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/images", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/lib", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/favicon", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        // Récupérer les infos de la visite
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var url = context.Request.Path + context.Request.QueryString;
        var referrer = context.Request.Headers["Referer"].ToString();
        
        var routeData = context.Request.RouteValues;
        var controller = routeData["controller"]?.ToString();
        var action = routeData["action"]?.ToString();

        // Vérifier si c'est une visite unique (basé sur la session)
        var isUnique = !context.Session.Keys.Contains("_HasVisited");
        if (isUnique)
        {
            context.Session.SetString("_HasVisited", "true");
        }

        // Enregistrer la visite de manière asynchrone (ne pas bloquer la requête)
        _ = Task.Run(async () =>
        {
            try
            {
                using var scope = context.RequestServices.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<AnalyticsService>();
                await service.EnregistrerVisiteAsync(ipAddress, userAgent, url, referrer, controller, action, isUnique);
            }
            catch
            {
                // Ignorer les erreurs pour ne pas bloquer la requête
            }
        });

        await _next(context);
    }
}


