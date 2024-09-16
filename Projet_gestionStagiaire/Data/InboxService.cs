using Projet_gestionStagiaire.Models;

namespace Projet_gestionStagiaire.Data
{
    public class InboxService
    {
        private readonly ApplicationDbContext _context;

        public InboxService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AjouterMessageAsync(string destinataire, string corps)
        {
            var message = new Inbox
            {
                Destinataire = destinataire,
                Corps = corps
            };

            _context.Inboxs.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
