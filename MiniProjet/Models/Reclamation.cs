using System;
using System.Collections.Generic;

namespace MiniProjet.Models;

public partial class Reclamation
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public DateTime DateReclamation { get; set; }

    public int IdArticleReclamation { get; set; }

    public int EtatId { get; set; }

    public int ClientId { get; set; }

    public int? InterventionId { get; set; }

    public virtual User Client { get; set; } = null!;

    public virtual Etat Etat { get; set; } = null!;

    public virtual Article IdArticleReclamationNavigation { get; set; } = null!;

    public virtual Intervention? Intervention { get; set; }
}
