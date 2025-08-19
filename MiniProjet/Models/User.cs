using System;
using System.Collections.Generic;

namespace MiniProjet.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string UserType { get; set; } = null!;

    public virtual ICollection<Reclamation> Reclamations { get; set; } = new List<Reclamation>();
}
