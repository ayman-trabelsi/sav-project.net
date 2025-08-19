using System;
using System.Collections.Generic;

namespace MiniProjet.Models;

public partial class Intervention
{
    public int Id { get; set; }

    public DateTime DateIntervention { get; set; }

    public string Description { get; set; } = null!;

    public decimal Prix { get; set; }

    public decimal MontantFacture { get; set; }

    public int TechnicienId { get; set; }

    public int ReclamationId { get; set; }

    public virtual Reclamation? Reclamation { get; set; }

    public virtual Technicien Technicien { get; set; } = null!;
}
