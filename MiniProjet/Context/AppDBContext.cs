using Microsoft.EntityFrameworkCore;
using Shared.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MiniProjet.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }

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
            base.OnModelCreating(modelBuilder);

            // Configure User inheritance
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<User>("User")
                .HasValue<Client>("Client")
                .HasValue<ResponsableSAV>("ResponsableSAV");

            // Configure Technicien entity
            modelBuilder.Entity<Technicien>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nom).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Telephone).IsRequired();
                entity.Property(e => e.Specialite).IsRequired();
            });

            // Configure Reclamation entity
            modelBuilder.Entity<Reclamation>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // Configure relationship with Article
                entity.HasOne(e => e.article)
                    .WithMany()
                    .HasForeignKey(e => e.idArticleReclamation)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure relationship with Etat
                entity.HasOne(e => e.Etat)
                    .WithMany()
                    .HasForeignKey(e => e.EtatId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure relationship with Client
                entity.HasOne(e => e.Client)
                    .WithMany()
                    .HasForeignKey(e => e.ClientId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired();
                entity.Property(e => e.UserType).IsRequired();
            });

            // Configure Article entity
            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Libelle).IsRequired();
                entity.Property(e => e.Prix).HasPrecision(18, 2);
            });

            // Configure PieceRechange entity
            modelBuilder.Entity<PieceRechange>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nom).IsRequired();
                entity.Property(e => e.Prix).HasPrecision(18, 2);

                entity.HasOne(e => e.Article)
                    .WithMany(a => a.PiecesRechanges)
                    .HasForeignKey(e => e.ArticleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Etat entity
            modelBuilder.Entity<Etat>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Libelle).IsRequired();
            });

            // Configure Intervention entity
            modelBuilder.Entity<Intervention>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DateIntervention).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Prix).HasPrecision(18, 2);
                entity.Property(e => e.MontantFacture).HasPrecision(18, 2);

                // Configure relationship with Technicien
                entity.HasOne(e => e.Technicien)
                    .WithMany(t => t.Interventions)
                    .HasForeignKey(e => e.TechnicienId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure relationship with Reclamation
                entity.HasOne(e => e.Reclamation)
                    .WithOne(r => r.Intervention)
                    .HasForeignKey<Intervention>(e => e.ReclamationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data for Etat
            modelBuilder.Entity<Etat>().HasData(
                new Etat { Id = 1, Libelle = "En Attente" },
                new Etat { Id = 2, Libelle = "Traité" },
                new Etat { Id = 3, Libelle = "En Cours" }
            );

            // Set decimal precision
            modelBuilder.Entity<Article>()
                .Property(a => a.Prix)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PieceRechange>()
                .Property(p => p.Prix)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Intervention>()
                .Property(i => i.MontantFacture)
                .HasPrecision(18, 2);
        }

    }
}
