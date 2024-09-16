using System.ComponentModel.DataAnnotations;

namespace Projet_gestionStagiaire.Models
{
    public class SujetDeStage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Titre { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int Duree { get; set; } // Durée du stage en mois ou semaines, à ajuster selon vos besoins

        // Relation avec Encadrant
        public int EncadrantId { get; set; }  // Foreign key vers Encadrant
        public Encadrant? Encadrant { get; set; }  // Propriété de navigation

        public ICollection<Stagiaire> Stagiaires { get; set; } = new List<Stagiaire>();
        public ICollection<Tache>? Taches { get; set; } = new List<Tache>();

    }
}
