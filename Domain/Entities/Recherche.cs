using System;

namespace Neovore.Web.Domain.Entities;

public class Recherche : BaseEntity
{
    public string Terme { get; set; } = "";
    public string? IpAddress { get; set; }
    public DateTime DateRecherche { get; set; } = DateTime.UtcNow;
    public int? ResultatsCount { get; set; } // Nombre de résultats trouvés
}


