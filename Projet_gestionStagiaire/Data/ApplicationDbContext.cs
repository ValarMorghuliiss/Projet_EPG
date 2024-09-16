using Microsoft.EntityFrameworkCore;
using Projet_gestionStagiaire.Models;

namespace Projet_gestionStagiaire.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Candidat> Candidats { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public DbSet<Encadrant> Encadrants { get; set; }

        public DbSet<Stagiaire> Stagiaires { get; set; }
        public DbSet<SujetDeStage> SujetsDeStage { get; set; }
        public DbSet<Tache> Taches { get; set; }
        public DbSet<Inbox> Inboxs { get; set; }




        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stagiaire>()
                .HasOne(s => s.Encadrant)
                .WithMany(e => e.Stagiaires)
                .HasForeignKey(s => s.EncadrantId);

            modelBuilder.Entity<SujetDeStage>()
                .HasOne(s => s.Encadrant)
                .WithMany(e => e.SujetsDeStage)
                .HasForeignKey(s => s.EncadrantId);

            modelBuilder.Entity<Stagiaire>()
                .HasOne(s => s.SujetDeStage)
                .WithMany(sj => sj.Stagiaires)
                .HasForeignKey(s => s.SujetDeStageId)
                .IsRequired(false); // This makes the foreign key optional


            // Relation entre Tache et Stagiaire (One-to-Many)
            modelBuilder.Entity<Tache>()
                .HasOne(t => t.Stagiaire)
                .WithMany(s => s.Taches)
                .HasForeignKey(t => t.StagiaireId)
                .OnDelete(DeleteBehavior.Restrict);  // Pas de suppression en cascade

            // Relation entre Tache et SujetDeStage (One-to-Many)
            modelBuilder.Entity<Tache>()
                .HasOne(t => t.SujetDeStage)
                .WithMany(s => s.Taches)
                .HasForeignKey(t => t.SujetDeStageId)
                .OnDelete(DeleteBehavior.Restrict);  // Pas de suppression en cascade

            base.OnModelCreating(modelBuilder);
        }
    }
}
