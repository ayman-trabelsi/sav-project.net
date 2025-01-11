using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniProjet.Models
{
    public class Article
    {
        public int Id { get; set; } 
        public string Libelle { get; set; }
        public bool EstSousGarantie { get; set; }
        public decimal Prix { get; set; }
        
    }
}
