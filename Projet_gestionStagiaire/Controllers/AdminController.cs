﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projet_gestionStagiaire.Migrations;
using Projet_gestionStagiaire.Models;
using Projet_gestionStagiaire.Repository;
using System.Security.Cryptography;
using System.Text;
using Admin = Projet_gestionStagiaire.Models.Admin;
using Candidat = Projet_gestionStagiaire.Models.Candidat;

namespace Projet_gestionStagiaire.Controllers
{
    public class AdminController : Controller
    {

        private readonly IRepository<Admin> _adminRepository;
        private readonly IRepository<Candidat> _candidatRepository;
        private readonly IRepository<Encadrant> _encadrantRepository;
        private readonly IRepository<Stagiaire> _stagiaireRepository;
        public AdminController(IRepository<Admin> adminRepository, IRepository<Candidat> candidatRepository, IRepository<Encadrant> encadrantRepository, IRepository<Stagiaire> stagiaireRepository)
        {
            _adminRepository = adminRepository;
            _candidatRepository = candidatRepository;
            _encadrantRepository = encadrantRepository;
            _stagiaireRepository = stagiaireRepository;
        }
       
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Admin admin)
        {
            var existingAdmin = (await _adminRepository.GetAllAsync())
                                .FirstOrDefault(a => a.Username == admin.Username);

            if (existingAdmin != null && VerifyPassword(existingAdmin, admin.Password))
            {
                return RedirectToAction("Dashboard");
            }

            ModelState.AddModelError("", "Nom d'utilisateur ou mot de passe incorrect");
            return View(admin);
        }


        public async Task<IActionResult> Dashboard()
        {
            var candidatsAvecStatutTrue = (await _candidatRepository.GetAllAsync())
                                          .Where(c => c.Status == true)
                                          .Count();
            var nombreDeEncadrants = (await _encadrantRepository.GetAllAsync()).Count();
            var nombreStagiares = (await _stagiaireRepository.GetAllAsync()).Count();

            var stagiaires = await _stagiaireRepository.GetAllAsync();
            var stagiairesParVille = stagiaires.GroupBy(s => s.Ville)
                                               .Select(group => new
                                               {
                                                   Ville = group.Key,
                                                   Count = group.Count()
                                               }).ToList();


            ViewBag.Villes = stagiairesParVille.Select(s => s.Ville).ToArray();
            ViewBag.Counts = stagiairesParVille.Select(s => s.Count).ToArray();

            ViewBag.NombreStagiares = nombreStagiares;
            ViewBag.NombreDeEncadrants = nombreDeEncadrants;
            ViewBag.NombreDeCandidats = candidatsAvecStatutTrue;
            ViewBag.StagiairesParVille = stagiairesParVille;



            return View();
        }

        public async Task<IActionResult> DemandeStage()
        {
            var demandes = (await _candidatRepository.GetAllAsync())
                                       .Where(c => c.Status == true)
                                       .ToList();

            return View(demandes);
        }

        [HttpPost]
        public async Task<IActionResult> BannerCandidat(int id)
        {
            var candidat = await _candidatRepository.GetByIdAsync(id);
            if (candidat != null && candidat.Status == true)
            {
                candidat.Status = false;
                _candidatRepository.Update(candidat);
                await _candidatRepository.SaveChangesAsync();
            }

            return RedirectToAction("DemandeStage");
        }


        public async Task<IActionResult> ListBanned()
        {
            var banned = (await _candidatRepository.GetAllAsync())
                                       .Where(c => c.Status == false)
                                       .ToList();
            return View(banned);
        }

        public async Task<IActionResult> AnnulerBanner(int id)
        {
            var candidat = await _candidatRepository.GetByIdAsync(id);
            if (candidat != null)
            {
                _candidatRepository.Delete(candidat);
                await _candidatRepository.SaveChangesAsync();
            }
            return RedirectToAction("ListBanned");
        }

        public async Task<IActionResult> Encadrant()
        {
            var encadrants = await _encadrantRepository.GetAllAsync();
            var stagiaires = await _stagiaireRepository.GetAllAsync();

            var nombreDeStagiairesParEncadrant = stagiaires
                .GroupBy(s => s.EncadrantId)
                .ToDictionary(g => g.Key, g => g.Count());

            ViewBag.NombreDeStagiairesParEncadrant = nombreDeStagiairesParEncadrant;
            return View(encadrants);
        }

        public IActionResult CreateEncadrant()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEncadrant(Encadrant encadrant)
        {
            if (ModelState.IsValid)
            {
                await _encadrantRepository.AddAsync(encadrant);
                await _encadrantRepository.SaveChangesAsync();
                return RedirectToAction("Encadrant");
            }
            return View(encadrant);
        }

        public async Task<IActionResult> DeleteEncadrant(int id)
        {
            var encadrant = await _encadrantRepository.GetByIdAsync(id);
            if (encadrant != null)
            {
                _encadrantRepository.Delete(encadrant);
                await _encadrantRepository.SaveChangesAsync();
            }

            return RedirectToAction("Encadrant");
        }

        
        [HttpGet]
        public async Task<IActionResult> AccepterStagiare(int id)
        {
            var candidat = await _candidatRepository.GetByIdAsync(id);


            var encadrants = await _encadrantRepository.GetAllAsync();
            ViewBag.Encadrants = encadrants.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = $"{e.Prenom} {e.Nom}"
            }).ToList();

            return View(candidat);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmerStagiare(int id, DateTime dateDebut, int DureDeStage, int encadrantId)
        {
            if (ModelState.IsValid)
            {
                var candidat = await _candidatRepository.GetByIdAsync(id);

                var stagiaire = new Stagiaire
                {
                    Nom = candidat.Nom,
                    Prenom = candidat.Prenom,
                    Email = candidat.Email,
                    Ville = candidat.Ville,
                    Telephone = candidat.Telephone,
                    AnneeUniversitaire = candidat.AnneeUniversitaire,
                    Universite = candidat.Universite,
                    DateDebut = dateDebut,
                    DureDeStage = DureDeStage,
                    EncadrantId = encadrantId
                };

                await _stagiaireRepository.AddAsync(stagiaire);
                await _stagiaireRepository.SaveChangesAsync();

                _candidatRepository.Delete(candidat);
                await _candidatRepository.SaveChangesAsync();

                return RedirectToAction("Dashboard");
            }

            return View("AccepterStagiare", id);
        }

        public async Task<IActionResult> Stagiaire()
        {
            var stagiaires = await _stagiaireRepository.GetAllWithIncludesAsync(s => s.Encadrant);

            foreach (var stagiaire in stagiaires)
            {
                if (stagiaire.DateFin < DateTime.Today && stagiaire.StatusStage == false)
                {
                    stagiaire.StatusStage = true; 
                    _stagiaireRepository.Update(stagiaire);
                    await _stagiaireRepository.SaveChangesAsync();
                }
            }

            return View(stagiaires);
        }

        public async Task<IActionResult> SettingProfil()
        {
            var admin = (await _adminRepository.GetAllAsync()).FirstOrDefault();
            ViewBag.AdminEmail = admin.Username;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SettingProfil(string currentPassword, string newPassword, string confirmNewPassword)
        {
            var admin = (await _adminRepository.GetAllAsync()).FirstOrDefault();
            ViewBag.AdminEmail = admin.Username;

            if (!VerifyPassword(admin, currentPassword))
            {
                ModelState.AddModelError("", "Le mot de passe actuel est incorrect.");
                return View();
            }

            if (newPassword != confirmNewPassword)
            {
                ModelState.AddModelError("", "La confirmation du nouveau mot de passe ne correspond pas.");
                return View();
            }


            admin.Password = HashPassword(newPassword);
            await _adminRepository.SaveChangesAsync();

            TempData["SuccessMessage"] = "Mot de passe changé avec succès.";
            return RedirectToAction("SettingProfil");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(Admin admin, string password)
        {
            return admin.Password == HashPassword(password);
        }


        public async Task<IActionResult> DeleteStagiaire(int id)
        {
            var stagiaire = await _stagiaireRepository.GetByIdAsync(id);
            if (stagiaire != null)
            {
                _stagiaireRepository.Delete(stagiaire);
                await _stagiaireRepository.SaveChangesAsync();
            }

            return RedirectToAction("Stagiaire");
        }

    }
}
