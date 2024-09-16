using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Projet_gestionStagiaire.Models
{
    public class Tache
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
        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }  

        [Required]
        public bool EstTerminee { get; set; } = false;

        public int? StagiaireId { get; set; }

        [ForeignKey("StagiaireId")]
        public Stagiaire? Stagiaire { get; set; }

        public int? SujetDeStageId { get; set; }

        [ForeignKey("SujetDeStageId")]
        public SujetDeStage? SujetDeStage { get; set; }
    }
}
