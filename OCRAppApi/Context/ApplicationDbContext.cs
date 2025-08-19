using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace OCRAppApi.Context
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ILogger<ApplicationDbContext> _logger;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Reclamation> Reclamations { get; set; }
        public DbSet<Etat> Etats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.LogTo(message => _logger.LogInformation(message));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).HasDefaultValue("Client");
            });

            // Configure Article entity
            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Libelle).IsRequired();
                entity.Property(e => e.EstSousGarantie).HasDefaultValue(false);
                entity.Property(e => e.Prix).HasPrecision(18, 2);

                // Seed data
                entity.HasData(
                    new Article { Id = 1, Libelle = "Ordinateur Portable", EstSousGarantie = true, Prix = 999.99m },
                    new Article { Id = 2, Libelle = "Smartphone", EstSousGarantie = true, Prix = 499.99m },
                    new Article { Id = 3, Libelle = "Tablette", EstSousGarantie = false, Prix = 299.99m }
                );
            });

            // Configure Etat entity
            modelBuilder.Entity<Etat>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Libelle).IsRequired();

                // Seed data
                entity.HasData(
                    new Etat { Id = 1, Libelle = "En Attente" },
                    new Etat { Id = 2, Libelle = "Traité" },
                    new Etat { Id = 3, Libelle = "En Cours" }
                );
            });

            // Configure Reclamation entity
            modelBuilder.Entity<Reclamation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.DateReclamation).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.EtatId).HasDefaultValue(1);
                
                // Configure relationships
                entity.HasOne(e => e.article)
                    .WithMany()
                    .HasForeignKey(e => e.idArticleReclamation)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Etat)
                    .WithMany()
                    .HasForeignKey(e => e.EtatId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Client)
                    .WithMany()
                    .HasForeignKey(e => e.ClientId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
} 