using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Libelle { get; set; } = string.Empty;
        public bool EstSousGarantie { get; set; }
        public decimal Prix { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? DateAchat { get; set; }
        public int? ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client? Client { get; set; }
        public virtual ICollection<PieceRechange> PiecesRechanges { get; set; } = new List<PieceRechange>();
    }
}
