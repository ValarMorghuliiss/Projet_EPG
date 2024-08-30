using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Projet_gestionStagiaire.Data
{
    public class CityService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CityService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<List<string>> GetCitiesAsync()
        {
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "ma.json");

            var jsonData = await File.ReadAllTextAsync(filePath);

            // Désérialiser en une liste de JObjects
            var citiesData = JsonConvert.DeserializeObject<List<JObject>>(jsonData);

            // Extraire les noms des villes
            var cityNames = new List<string>();
            foreach (var city in citiesData)
            {
                // Assurez-vous que la propriété 'City' existe dans chaque objet
                if (city["city"] != null)
                {
                    cityNames.Add(city["city"].ToString());
                }
            }

            return cityNames;
        }
    }
}
