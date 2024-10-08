﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_gestionStagiaire.Models
{
    public class Encadrant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Nom { get; set; }

        [Required]
        [StringLength(30)]
        public string Prenom { get; set; }

        [Required]
        [StringLength(100)]
        public string MotDePasse { get; set; }

        [NotMapped]
        public string Username
        {
            get
            {
                return $"{Prenom.ToLower()}.{Nom.ToLower()}";
            }
        }

        public Encadrant()
        {
            MotDePasse = GenererMotDePasse();
        }

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
        [Required]
        [EmailAddress]
        public string Email { get; set; }  // Nouvelle propriété

        public ICollection<Stagiaire>? Stagiaires { get; set; }

        public ICollection<SujetDeStage>? SujetsDeStage { get; set; }
    }
}
