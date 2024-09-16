using System.ComponentModel.DataAnnotations;

namespace Projet_gestionStagiaire.Models
{
    public class Inbox
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Destinataire { get; set; }

        [Required]
        public string Corps { get; set; }

        public bool StatusDeInbox { get; set; } = false;
    }
}
