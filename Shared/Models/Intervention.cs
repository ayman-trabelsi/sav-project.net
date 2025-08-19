using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class Intervention
    {
        public int Id { get; set; }
        public DateTime DateIntervention { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Prix { get; set; }
        public decimal MontantFacture { get; set; }
        public decimal MainDOeuvre { get; set; }
        public int TechnicienId { get; set; }
        [ForeignKey("TechnicienId")]
        public Technicien? Technicien { get; set; }
        public int ReclamationId { get; set; }
        [ForeignKey("ReclamationId")]
        public Reclamation? Reclamation { get; set; }
    }
}
