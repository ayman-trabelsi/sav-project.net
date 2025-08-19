using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MiniProjet.Models;

public partial class SavdbContext : DbContext
{
    public SavdbContext()
    {
    }

    public SavdbContext(DbContextOptions<SavdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Etat> Etats { get; set; }

    public virtual DbSet<Intervention> Interventions { get; set; }

    public virtual DbSet<PiecesRechange> PiecesRechanges { get; set; }

    public virtual DbSet<Reclamation> Reclamations { get; set; }

    public virtual DbSet<Technicien> Techniciens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SAVDB;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.Property(e => e.Prix).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Intervention>(entity =>
        {
            entity.HasIndex(e => e.TechnicienId, "IX_Interventions_TechnicienId");

            entity.Property(e => e.MontantFacture).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Prix).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Technicien).WithMany(p => p.Interventions).HasForeignKey(d => d.TechnicienId);
        });

        modelBuilder.Entity<PiecesRechange>(entity =>
        {
            entity.ToTable("PiecesRechange");

            entity.HasIndex(e => e.ArticleId, "IX_PiecesRechange_ArticleId");

            entity.Property(e => e.Prix).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Article).WithMany(p => p.PiecesRechanges).HasForeignKey(d => d.ArticleId);
        });

        modelBuilder.Entity<Reclamation>(entity =>
        {
            entity.HasIndex(e => e.ClientId, "IX_Reclamations_ClientId");

            entity.HasIndex(e => e.EtatId, "IX_Reclamations_EtatId");

            entity.HasIndex(e => e.InterventionId, "IX_Reclamations_InterventionId")
                .IsUnique()
                .HasFilter("([InterventionId] IS NOT NULL)");

            entity.HasIndex(e => e.IdArticleReclamation, "IX_Reclamations_idArticleReclamation");

            entity.Property(e => e.IdArticleReclamation).HasColumnName("idArticleReclamation");

            entity.HasOne(d => d.Client).WithMany(p => p.Reclamations)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Etat).WithMany(p => p.Reclamations)
                .HasForeignKey(d => d.EtatId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdArticleReclamationNavigation).WithMany(p => p.Reclamations)
                .HasForeignKey(d => d.IdArticleReclamation)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Intervention).WithOne(p => p.Reclamation).HasForeignKey<Reclamation>(d => d.InterventionId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserType).HasMaxLength(21);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
