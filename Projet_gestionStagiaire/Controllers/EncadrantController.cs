using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet_gestionStagiaire.Data;
using Projet_gestionStagiaire.Models;
using Projet_gestionStagiaire.Repository;

namespace Projet_gestionStagiaire.Controllers
{
    public class EncadrantController : Controller
    {
        private readonly IRepository<Encadrant> _encadrantRepository;
        private readonly IRepository<Stagiaire> _stagiaireRepository;

        private readonly IRepository<Tache> _tacheRepository;
        private readonly IRepository<SujetDeStage> _sujetDeStageRepository;
        private readonly IRepository<Inbox> _inboxRepository;


        private readonly ApplicationDbContext _context;


        public EncadrantController(IRepository<Inbox> inboxRepository,ApplicationDbContext context, IRepository<Stagiaire> stagiaireRepository, IRepository<Tache> tacheRepository, IRepository<Encadrant> encadrantRepository, IRepository<SujetDeStage> sujetdeStageRepository)
        {
            _context = context;
            _encadrantRepository = encadrantRepository;
            _sujetDeStageRepository = sujetdeStageRepository;
            _tacheRepository = tacheRepository;
            _stagiaireRepository = stagiaireRepository;
            _inboxRepository = inboxRepository;

        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Veuillez entrer votre nom d'utilisateur et mot de passe.";
                return View();
            }

            var encadrant = await _encadrantRepository.FindAsync(e => e.Prenom.ToLower() + "." + e.Nom.ToLower() == username.ToLower());

            if (encadrant == null || encadrant.MotDePasse != password)
            {
                ViewBag.Error = "Nom d'utilisateur ou mot de passe incorrect.";
                return View();
            }

            HttpContext.Session.SetInt32("EncadrantId", encadrant.Id);

            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> Dashboard()
        {
            int? encadrantId = HttpContext.Session.GetInt32("EncadrantId");

            if (encadrantId == null)
            {
                return RedirectToAction("Login");
            }

            var encadrant = await _encadrantRepository.GetByIdAsync(encadrantId.Value);

            var numberOfStagiaires = await _context.Stagiaires.CountAsync(s => s.EncadrantId == encadrantId.Value);
            var numberOfSujetStagiare = await _context.SujetsDeStage.CountAsync(s => s.EncadrantId == encadrantId.Value);
            var unreadMessages = await _inboxRepository.FindAllAsync(i => i.Destinataire == encadrant.Username && !i.StatusDeInbox);
            ViewBag.NumberOfUnreadMessages = unreadMessages.Count();

            ViewBag.NumberOfStagiaires = numberOfStagiaires;
            ViewBag.NumberOfSujetStagiare = numberOfSujetStagiare;

            ViewBag.EncadrantNom = encadrant.Prenom + " " + encadrant.Nom;


            return View(encadrant);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _inboxRepository.GetByIdAsync(id);
            if (message != null)
            {
                _inboxRepository.Delete(message);
                await _inboxRepository.SaveChangesAsync();
            }

            return RedirectToAction("Inbox");
        }
        public async Task<IActionResult> Inbox()
        {

            int? encadrantId = HttpContext.Session.GetInt32("EncadrantId");

            if (encadrantId == null)
            {
                return RedirectToAction("Login");
            }

            var encadrant = await _encadrantRepository.GetByIdAsync(encadrantId.Value);
            ViewBag.EncadrantNom = encadrant.Prenom + " " + encadrant.Nom;

            // Récupérer les messages pour cet encadrant
            var messages = await _inboxRepository.FindAllAsync(i => i.Destinataire == encadrant.Username);
            ViewBag.Messages = messages;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var message = await _inboxRepository.GetByIdAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            message.StatusDeInbox = true; // Mark as read
            _inboxRepository.Update(message);
            await _inboxRepository.SaveChangesAsync();

            return RedirectToAction("Inbox"); // Redirect to the inbox view
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("EncadrantId");
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Stagiaires()
        {
            int? encadrantId = HttpContext.Session.GetInt32("EncadrantId");

            if (encadrantId == null)
            {
                return RedirectToAction("Login");
            }

            var encadrant = await _encadrantRepository.GetByIdAsync(encadrantId.Value);
            ViewBag.EncadrantNom = encadrant.Prenom + " " + encadrant.Nom;

            var stagiaires = await _context.Stagiaires
                .Where(s => s.EncadrantId == encadrantId.Value)
                .Include(s => s.SujetDeStage) // Assurer que le SujetDeStage est inclus
                .ToListAsync();

            return View(stagiaires);
        }

        public async Task<IActionResult> AjouterSujetDestage()
        {
            int? encadrantId = HttpContext.Session.GetInt32("EncadrantId");

            if (encadrantId == null)
            {
                return RedirectToAction("Login");
            }

            var encadrant = await _encadrantRepository.GetByIdAsync(encadrantId.Value);
            ViewBag.EncadrantNom = encadrant.Prenom + " " + encadrant.Nom;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AjouterSujetDestage(SujetDeStage sujetDeStage)
        {
            int? encadrantId = HttpContext.Session.GetInt32("EncadrantId");

            if (encadrantId == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                sujetDeStage.EncadrantId = encadrantId.Value;
                await _sujetDeStageRepository.AddAsync(sujetDeStage);
                await _sujetDeStageRepository.SaveChangesAsync();
                return RedirectToAction("SujetdeStage");
            }

            return View(sujetDeStage);
        }

        public async Task<IActionResult> SujetdeStage()
        {
            int? encadrantId = HttpContext.Session.GetInt32("EncadrantId");

            if (encadrantId == null)
            {
                return RedirectToAction("Login");
            }
            var encadrant = await _encadrantRepository.GetByIdAsync(encadrantId.Value);
            ViewBag.EncadrantNom = encadrant.Prenom + " " + encadrant.Nom;

            var sujetsDeStage = await _sujetDeStageRepository.GetAllWithIncludesAsync(s => s.Encadrant);
            var sujetsEncadrant = sujetsDeStage.Where(s => s.EncadrantId == encadrantId.Value).ToList();

            return View(sujetsEncadrant);
        }

        public async Task<IActionResult> ShowAssignSubject(int stagiaireId)
        {
            int? encadrantId = HttpContext.Session.GetInt32("EncadrantId");

            if (encadrantId == null)
            {
                return RedirectToAction("Login");
            }
            var sujetsDeStage = await _sujetDeStageRepository.GetAllAsync();

            var stagiaire = await _context.Stagiaires.FindAsync(stagiaireId);

            // Passer les données à la vue via ViewBag
            ViewBag.Stagiaire = stagiaire;
            ViewBag.SujetsDeStage = sujetsDeStage;


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignSubject(int stagiaireId, int sujetDeStageId)
        {
            int? encadrantId = HttpContext.Session.GetInt32("EncadrantId");

            if (encadrantId == null)
            {
                return RedirectToAction("Login");
            }
            var stagiaire = await _context.Stagiaires.FindAsync(stagiaireId);
            var sujet = await _sujetDeStageRepository.GetByIdAsync(sujetDeStageId);
            var encadrant = await _encadrantRepository.GetByIdAsync(encadrantId.Value);
            ViewBag.EncadrantNom = encadrant.Prenom + " " + encadrant.Nom;

            if (stagiaire == null || sujet == null)
            {
                return NotFound();
            }

            stagiaire.SujetDeStage = sujet;
            await _context.SaveChangesAsync();

            return RedirectToAction("Stagiaires");
        }

        public async Task<IActionResult> TachesAsync(int stagiaireId, int sujetDeStageId)
        {
            int? encadrantId = HttpContext.Session.GetInt32("EncadrantId");

            if (encadrantId == null)
            {
                return RedirectToAction("Login");
            }

            var encadrant = await _encadrantRepository.GetByIdAsync(encadrantId.Value);
            ViewBag.EncadrantNom = encadrant.Prenom + " " + encadrant.Nom;

            var stagiaire = await _stagiaireRepository.GetByIdAsync(stagiaireId);
            var sujetDeStage = await _sujetDeStageRepository.GetByIdAsync(sujetDeStageId);

            var taches = await _tacheRepository.GetAllWithIncludesAsync(
                t => t.Stagiaire, t => t.SujetDeStage
            );

            var tachesFiltrees = taches
                .Where(t => t.StagiaireId == stagiaireId && t.SujetDeStageId == sujetDeStageId)
                .OrderBy(t => t.DateDebut); // Tri par DateDebut en ordre croissant

            ViewBag.Taches = tachesFiltrees;

            // Calculer les statistiques
            var totalTasks = tachesFiltrees.Count();
            var plannedTasks = tachesFiltrees.Count(t => !t.EstTerminee && t.DateDebut.Date > DateTime.Today);
            var inProgressTasks = tachesFiltrees.Count(t => !t.EstTerminee && t.DateDebut.Date <= DateTime.Today);
            var completedTasks = tachesFiltrees.Count(t => t.EstTerminee);

            // Passer les statistiques à la vue via ViewBag ou ViewModel
            ViewBag.PlannedTasks = plannedTasks;
            ViewBag.InProgressTasks = inProgressTasks;
            ViewBag.CompletedTasks = completedTasks;

            ViewBag.StagiaireId = stagiaireId;
            ViewBag.sujetDeStageId = sujetDeStageId;

            return View();
        }

        public async Task<IActionResult> AjouterTacheAsync(int stagiaireId, int sujetDeStageId)
        {
            int? encadrantId = HttpContext.Session.GetInt32("EncadrantId");
            if (encadrantId == null)
            {
                return RedirectToAction("Login");
            }
            var encadrant = await _encadrantRepository.GetByIdAsync(encadrantId.Value);
            ViewBag.EncadrantNom = encadrant.Prenom + " " + encadrant.Nom;

            ViewBag.StagiaireId = stagiaireId;
            ViewBag.SujetDeStageId = sujetDeStageId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AjouterTacheAsync(Tache tache)
        {
            // Récupère l'ID de l'encadrant depuis la session
            int? encadrantId = HttpContext.Session.GetInt32("EncadrantId");
            if (encadrantId == null)
            {
                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid)
            {
                var encadrant = await _encadrantRepository.GetByIdAsync(encadrantId.Value);
                ViewBag.EncadrantNom = encadrant.Prenom + " " + encadrant.Nom;
                ViewBag.StagiaireId = tache.StagiaireId;
                ViewBag.SujetDeStageId = tache.SujetDeStageId;
                return View(tache);
            }
            tache.SujetDeStageId = tache.SujetDeStageId;
            tache.StagiaireId = tache.StagiaireId;
            tache.Stagiaire = await _stagiaireRepository.GetByIdAsync(tache.StagiaireId.Value);
            tache.SujetDeStage = await _sujetDeStageRepository.GetByIdAsync(tache.SujetDeStageId.Value);

            await _tacheRepository.AddAsync(tache);
            await _tacheRepository.SaveChangesAsync();

            return RedirectToAction("Taches", new { stagiaireId = tache.StagiaireId, sujetDeStageId = tache.SujetDeStageId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(int id, int stagiaireId, int sujetDeStageId)
        {
            var tache = await _tacheRepository.GetByIdAsync(id);
            if (tache != null)
            {
                _tacheRepository.Delete(tache);
                await _tacheRepository.SaveChangesAsync();
            }

            // Redirection vers la vue des tâches avec les paramètres stagiaireId et sujetDeStageId
            return RedirectToAction("Taches", new { stagiaireId = stagiaireId, sujetDeStageId = sujetDeStageId });
        }


        public async Task<IActionResult> TachDetails(int id)
        {
            int? encadrantId = HttpContext.Session.GetInt32("EncadrantId");
            if (encadrantId == null)
            {
                return RedirectToAction("Login");
            }
            var encadrant = await _encadrantRepository.GetByIdAsync(encadrantId.Value);
            ViewBag.EncadrantNom = encadrant.Prenom + " " + encadrant.Nom;

            var tache = await _tacheRepository.GetByIdAsync(id);
            return View(tache);
        }


    }
}

