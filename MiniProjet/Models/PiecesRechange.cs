using System;
using System.Collections.Generic;

namespace MiniProjet.Models;

public partial class PiecesRechange
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public decimal Prix { get; set; }

    public int ArticleId { get; set; }

    public virtual Article Article { get; set; } = null!;
}
