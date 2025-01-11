
using Microsoft.EntityFrameworkCore;
using MiniProjet.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OCRAppApi.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ResponsableSAV> ResponsableSAV { get; set; }
        public DbSet<Reclamation> Reclamations { get; set; }
        public DbSet<Intervention> Interventions { get; set; }
        public DbSet<Technicien> Techniciens { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<PieceRechange> PiecesRechange { get; set; }
        public DbSet<Etat> Etats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Etat>().HasData(
              new Etat { Id = 1, Libelle = "En Attente" },
              new Etat { Id = 2, Libelle = "Traité" },
              new Etat { Id = 3, Libelle = "En Cours" }
          );

        }
    }
}
