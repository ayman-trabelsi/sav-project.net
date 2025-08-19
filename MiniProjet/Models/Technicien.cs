using System;
using System.Collections.Generic;

namespace MiniProjet.Models;

public partial class Technicien
{
    public int Id { get; set; }

    public string Nom { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telephone { get; set; } = null!;

    public virtual ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();
}
