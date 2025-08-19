using System;
using System.Collections.Generic;

namespace MiniProjet.Models;

public partial class Etat
{
    public int Id { get; set; }

    public string Libelle { get; set; } = null!;

    public virtual ICollection<Reclamation> Reclamations { get; set; } = new List<Reclamation>();
}
