using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_gestionStagiaire.Models
{
    public class Candidat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nom { get; set; }

        [Required]
        [StringLength(50)]
        public string Prenom { get; set; }

        [Required]
        public DateTime DateCandidature { get; set; } = DateTime.Now;

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        public bool Status { get; set; } = true;

        [Required]
        [StringLength(50)]
        public string Ville { get; set; }

        [Required]
        [StringLength(100)]
        public string Universite { get; set; }

        [Required]
        [Range(1, 5)]
        public int AnneeUniversitaire { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Veuillez entrer un numéro de téléphone valide (10 chiffres).")]
        public string Telephone { get; set; }

        [Required]
        [StringLength(20)]
        public string NumeroIdentite { get; set; }

        [Required]
        [StringLength(50)]
        public string Domaine { get; set; }

        [NotMapped]
        public IFormFile? File { get; set; }

        public string? FilePath { get; set; }

        [NotMapped]
        public IFormFile? Pdf { get; set; }
        public string? PdfPath { get; set; }
    }
}