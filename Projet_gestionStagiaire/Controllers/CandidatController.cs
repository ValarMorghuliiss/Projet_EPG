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

        public CandidatController(IRepository<Candidat> candidatRepository, CityService cityService)
        {
            _candidatRepository = candidatRepository;
            _cityService = cityService;
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
            if (!ModelState.IsValid)
            {
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
          
            
            await _candidatRepository.AddAsync(candidat);
            await _candidatRepository.SaveChangesAsync();

            return View("Success");
        }

    }
}
