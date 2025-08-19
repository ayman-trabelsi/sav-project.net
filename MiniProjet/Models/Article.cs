using System;
using System.Collections.Generic;

namespace MiniProjet.Models;

public partial class Article
{
    public int Id { get; set; }

    public string Libelle { get; set; } = null!;

    public bool EstSousGarantie { get; set; }

    public decimal Prix { get; set; }

    public virtual ICollection<PiecesRechange> PiecesRechanges { get; set; } = new List<PiecesRechange>();

    public virtual ICollection<Reclamation> Reclamations { get; set; } = new List<Reclamation>();
}
