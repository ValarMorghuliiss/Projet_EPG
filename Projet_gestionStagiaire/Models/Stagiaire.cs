using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Projet_gestionStagiaire.Models
{
    public class Stagiaire
    {
        private string GenererMotDePasse(int longueur = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var motDePasse = new char[longueur];

            for (int i = 0; i < longueur; i++)
            {
                motDePasse[i] = chars[random.Next(chars.Length)];
            }

            return new string(motDePasse);
        }

        public Stagiaire()
        {
            MotDePasse = GenererMotDePasse();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        [Required]
        public string Prenom { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public bool StatusStage { get; set; } = false;

        [Required]
        public string Ville { get; set; }

        [Required]
        public string Universite { get; set; }

        [Required]
        public int AnneeUniversitaire { get; set; }

        [Required]
        public string Telephone { get; set; }

        [Required]
        public string MotDePasse { get; set; }

        [Required]
        public DateTime DateDebut { get; set; }

        [Required]
        public int DureDeStage { get; set; }

        // Relation avec Encadrant
        [Required]
        public int EncadrantId { get; set; }

        [ForeignKey("EncadrantId")]
        public Encadrant Encadrant { get; set; }

        [NotMapped]
        public DateTime DateFin => DateDebut.AddMonths(DureDeStage);

        [NotMapped]
        public string StatutAffichage
        {
            get
            {
                if (DateFin >= DateTime.Today)
                {
                    return "Stage en cours";
                }
                else
                {
                    StatusStage = true; //
                    return "Stage terminé";
                }
            }
        }
    }
}