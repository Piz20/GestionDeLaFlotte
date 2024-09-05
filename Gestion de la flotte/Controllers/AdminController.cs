using Gestion_de_la_flotte.Data;
using Gestion_de_la_flotte.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;

namespace Gestion_de_la_flotte.Controllers
{
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AdminDashboard()
        {
            // Vous pouvez ajouter toute logique nécessaire ici
            return View(); // Renvoie la vue AdminDashboard.cshtml
        }

        [HttpGet]
        public IActionResult AdminUserManagement()
        {
            // Vous pouvez ajouter toute logique nécessaire ici
            return View(); // Renvoie la vue AdminDashboard.cshtml
        }

        [HttpGet]
        public IActionResult AdminAddUser()
        {
            // Vous pouvez ajouter toute logique nécessaire ici
            return View(); // Renvoie la vue AdminDashboard.cshtml
        }
        // Méthode pour afficher le formulaire de création d'utilisateur
        [HttpGet]
        public IActionResult AdminAddFleetUser()
        {
            return View(); // Retourne la vue contenant le formulaire
        }

        // Méthode pour traiter la soumission du formulaire et créer un utilisateur dans la base de données
        [HttpPost]
        public async Task<IActionResult> AdminAddFleetUser(FleetUser fleetUser)
        {
            
            Console.WriteLine(fleetUser.ToString());

            if (ModelState.IsValid)
            {
                fleetUser.Id = Guid.NewGuid().ToString();

                fleetUser.CreationDate = DateTime.Now;
                fleetUser.Status = -1;
                _context.FleetUsers.Add(fleetUser);
                await _context.SaveChangesAsync();

                return View(fleetUser);
            }
            else
            {
              
                // Afficher les erreurs de validation
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            return View(fleetUser);
        }

    }
}
