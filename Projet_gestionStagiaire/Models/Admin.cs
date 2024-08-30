using System.ComponentModel.DataAnnotations;

namespace Projet_gestionStagiaire.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
       
        [EmailAddress]
        public string Username { get; set; }

        public string Password { get; set; }
    }
}