using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniProjet.Models
{
    public class PieceRechange
    {

        public int Id { get; set; }
        public string Nom { get; set; }
        public decimal Prix { get; set; }
        public int ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public Article? Article { get; set; }

    }
}
