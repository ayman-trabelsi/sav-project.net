using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class PieceRechange
    {
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; } = string.Empty;

        [Required]
        public decimal Prix { get; set; }

        [Required]
        public int ArticleId { get; set; }

        public string? ImageUrl { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article? Article { get; set; }
    }
}
