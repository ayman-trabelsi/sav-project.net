using System.Collections.Generic;

namespace Shared.Models
{
    public class Technicien
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Specialite { get; set; } = string.Empty;
        public virtual ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();
    }
}
