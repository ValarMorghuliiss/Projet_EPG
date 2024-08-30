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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stagiaire>()
                .HasOne(s => s.Encadrant)
                .WithMany(e => e.Stagiaires)
                .HasForeignKey(s => s.EncadrantId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
