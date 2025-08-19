using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class Reclamation
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public DateTime DateReclamation { get; set; }

        [Required]
        public int idArticleReclamation { get; set; }

        [ForeignKey("idArticleReclamation")]
        public Article? article { get; set; }

        [Required]
        public int EtatId { get; set; }

        [ForeignKey("EtatId")]
        public Etat? Etat { get; set; }

        [Required]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client? Client { get; set; }

        public int? InterventionId { get; set; }

        public Intervention? Intervention { get; set; }
    }
}
