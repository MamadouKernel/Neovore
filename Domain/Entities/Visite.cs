using System;

namespace Neovore.Web.Domain.Entities;

public class Visite : BaseEntity
{
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? Url { get; set; }
    public string? Referrer { get; set; }
    public string? Controller { get; set; }
    public string? Action { get; set; }
    public DateTime DateVisite { get; set; } = DateTime.UtcNow;
    public bool EstUnique { get; set; } = true; // Visite unique (première visite de la session)
}


