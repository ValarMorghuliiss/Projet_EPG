using System.Net.Mail;
using System.Net;

namespace Projet_gestionStagiaire.Data
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService()
        {
            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587, // Port SMTP
                Credentials = new NetworkCredential("marouane.mouhcine10@gmail.com", "fkpwrddouwdliayo"),
                EnableSsl = true
            };
        }
        public async Task SendWelcomeEmailAsync(string toEmail, string username, string password)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("marouane.mouhcine10@gmail.com"),
                Subject = "Bienvenue dans notre Centre de Formation EPG",
                Body = $"Bienvenue dans notre entreprise!\n\nVotre nom d'utilisateur: {username}\nVotre mot de passe: {password}",
                IsBodyHtml = false,
            };
            mailMessage.To.Add(toEmail);

            await _smtpClient.SendMailAsync(mailMessage);
        }

    }
}
