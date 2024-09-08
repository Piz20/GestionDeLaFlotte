using Gestion_de_la_flotte.Data; // Contexte de base de données
using Gestion_de_la_flotte.Models; // Modèles de données
using Microsoft.AspNetCore.Mvc; // Fonctionnalités MVC
using Microsoft.EntityFrameworkCore; // Fonctionnalités Entity Framework Core

namespace Gestion_de_la_flotte.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context; // Contexte de la base de données

        // Constructeur, initialise le contexte de la base de données
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Tableau de bord de l'administrateur
        [HttpGet]
        public IActionResult AdminDashboard()
        {
            return View();
        }

        // Gestion des utilisateurs
        [HttpGet]
        public IActionResult AdminUserManagement()
        {
            var fleetUsers = _context.FleetUsers.ToList(); // Récupère la liste des utilisateurs
            return View(fleetUsers);
        }

        // Ajout d'utilisateur (GET)
        [HttpGet]
        public IActionResult AdminAddFleetUser()
        {
            return View(); // Affiche le formulaire d'ajout d'utilisateur
        }

        // Ajout d'utilisateur (POST)
        [HttpPost]
        public async Task<IActionResult> AdminAddFleetUser(FleetUser fleetUser)
        {
            // Vérification des doublons
            bool matriculeExists = await _context.FleetUsers.AnyAsync(u => u.Matricule == fleetUser.Matricule);
            bool emailExists = await _context.FleetUsers.AnyAsync(u => u.Email == fleetUser.Email);
            bool cniExists = await _context.FleetUsers.AnyAsync(u => u.CNINumber == fleetUser.CNINumber);

            // Ajout des erreurs si nécessaire
            if (matriculeExists)
                ModelState.AddModelError("Matricule", "Ce matricule existe déjà.");
            if (emailExists)
                ModelState.AddModelError("Email", "Cet email est déjà utilisé.");
            if (cniExists)
                ModelState.AddModelError("CNINumber", "Ce numéro de CNI existe déjà.");

            // Si des erreurs existent, retourner la vue avec les erreurs
            if (!ModelState.IsValid)
                return View(fleetUser);

            // Si tout est valide, ajout de l'utilisateur
            fleetUser.Id = Guid.NewGuid().ToString(); // Génération d'un ID
            fleetUser.CreationDate = DateTime.Now;
            fleetUser.Status = -1; // Statut par défaut

            _context.FleetUsers.Add(fleetUser); // Ajout de l'utilisateur
            await _context.SaveChangesAsync(); // Sauvegarde dans la base

            TempData["SuccessMessage"] = "Utilisateur ajouté avec succès !";
            return RedirectToAction("AdminAddFleetUser");
        }

        // Suppression d'utilisateur
        [HttpPost]
        public IActionResult AdminDeleteFleetUser(string fleetUserId)
        {
            var fleetUser = _context.FleetUsers.Find(fleetUserId);
            if (fleetUser == null)
                return NotFound(); // Utilisateur non trouvé

            _context.FleetUsers.Remove(fleetUser);
            _context.SaveChanges();

            return RedirectToAction("AdminUserManagement");
        }

        // Modifier un utilisateur (GET)
        [HttpGet]
        public IActionResult AdminEditUser(string fleetUserId)
        {
            var fleetUser = _context.FleetUsers.Find(fleetUserId);
            Console.WriteLine(fleetUserId + "=================================");
            if (fleetUser == null)
                return NotFound(); // Utilisateur non trouvé

            return PartialView("_EditUserPartial", fleetUser); // Affiche une vue partielle
        }

        // Modifier un utilisateur (POST)
        [HttpPost]
        public IActionResult AdminEditUser(FleetUser model)
        {
            var fleetUser = _context.FleetUsers.Find(model.Id);
            if (fleetUser == null)
                return NotFound(); // Utilisateur non trouvé

            var errors = new Dictionary<string, string>();

            // Validation de l'unicité des champs
            if (_context.FleetUsers.Any(u => u.Email == model.Email && u.Id != model.Id))
                errors["Email"] = "L'email est déjà utilisé.";
            if (_context.FleetUsers.Any(u => u.CNINumber == model.CNINumber && u.Id != model.Id))
                errors["CNINumber"] = "Le numéro de CNI est déjà utilisé.";
            if (_context.FleetUsers.Any(u => u.Matricule == model.Matricule && u.Id != model.Id))
                errors["Matricule"] = "Le matricule est déjà utilisé.";

            if (errors.Any())
                return Json(new { errors });

            // Mettre à jour les propriétés de l'utilisateur
            fleetUser.Name = model.Name;
            fleetUser.SurName = model.SurName;
            fleetUser.Matricule = model.Matricule;
            fleetUser.CNINumber = model.CNINumber;
            fleetUser.Email = model.Email;
            fleetUser.Status = model.Status;

            _context.SaveChanges();

            return Json(new
            {
                id = fleetUser.Id,
                name = fleetUser.Name,
                surname = fleetUser.SurName,
                matricule = fleetUser.Matricule,
                cniNumber = fleetUser.CNINumber,
                email = fleetUser.Email,
                status = fleetUser.Status
            });

        }
    }
}