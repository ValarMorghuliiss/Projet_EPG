using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projet_gestionStagiaire.Data;
using Projet_gestionStagiaire.Models;
using Projet_gestionStagiaire.Repository;

namespace Projet_gestionStagiaire.Controllers
{
    public class CandidatController : Controller
    {
        private readonly IRepository<Candidat> _candidatRepository;
        private readonly CityService _cityService;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public CandidatController(IRepository<Candidat> candidatRepository, CityService cityService,IWebHostEnvironment webHostEnvironment)
        {
            _candidatRepository = candidatRepository;
            _cityService = cityService;
            _webHostEnvironment = webHostEnvironment;
        }
       
        public async Task<IActionResult> Inscription()
        {
            var cities = await _cityService.GetCitiesAsync();
            ViewBag.Villes = new SelectList(cities);

            ViewBag.Annees = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Première année" },
                new SelectListItem { Value = "2", Text = "Deuxième année" },
                new SelectListItem { Value = "3", Text = "Troisième année" },
                new SelectListItem { Value = "4", Text = "Quatrième année" },
                new SelectListItem { Value = "5", Text = "Cinquième année" }
            };

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Inscription(Candidat candidat)
        {
            // Vérifier si le modèle est valide
            if (!ModelState.IsValid)
            {
                // Remplir les ViewBag pour les listes déroulantes
                ViewBag.Annees = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Première année" },
            new SelectListItem { Value = "2", Text = "Deuxième année" },
            new SelectListItem { Value = "3", Text = "Troisième année" },
            new SelectListItem { Value = "4", Text = "Quatrième année" },
            new SelectListItem { Value = "5", Text = "Cinquième année" }
        };

                var cities = await _cityService.GetCitiesAsync();
                ViewBag.Villes = new SelectList(cities);

                return View(candidat);
            }

            // Vérifier si un candidat avec le même numéro d'identité existe déjà
            var existingCandidat = await _candidatRepository.FindAsync(c => c.NumeroIdentite == candidat.NumeroIdentite);
            if (existingCandidat != null)
            {
                if (existingCandidat.Status)
                {
                    return View("Traitement");
                }
                else
                {
                    return View("Interdit");
                }
            }
            
            if (candidat.File != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + candidat.File.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await candidat.File.CopyToAsync(fileStream);
                    }
                    candidat.FilePath = "/uploads/" + uniqueFileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Une erreur est survenue lors du téléchargement de l'image : " + ex.Message);
                    return View(candidat);
                }
            }

            if (candidat.Pdf != null)
            {
                var pdfFolder = Path.Combine(_webHostEnvironment.WebRootPath, "pdfs");
                var uniquePdfName = Guid.NewGuid().ToString() + "_" + candidat.Pdf.FileName;
                var pdfPath = Path.Combine(pdfFolder, uniquePdfName);

                try
                {
                    using (var pdfStream = new FileStream(pdfPath, FileMode.Create))
                    {
                        await candidat.Pdf.CopyToAsync(pdfStream);
                    }
                    candidat.PdfPath = "/pdfs/" + uniquePdfName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Une erreur est survenue lors du téléchargement du PDF : " + ex.Message);
                    return View(candidat);
                }
            }

            // Enregistrement du candidat
            await _candidatRepository.AddAsync(candidat);
            await _candidatRepository.SaveChangesAsync();

                return View("Success");
            }
        }
    }
