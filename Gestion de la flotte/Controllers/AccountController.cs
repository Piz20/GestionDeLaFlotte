using Gestion_de_la_flotte.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gestion_de_la_flotte.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Cette adresse email est déjà utilisée.");
                    // Retirer le mot de passe pour qu'il ne soit pas affiché en cas d'erreur
                    model.Password = string.Empty;
                    model.ConfirmPassword = string.Empty;
                    return View(model);
                }
                else
                {
                    var user = new User { UserName = model.Email, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }

                    // Ajouter les erreurs au ModelState pour les afficher
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    // Retirer le mot de passe pour qu'il ne soit pas affiché en cas d'erreur
                    model.Password = string.Empty;
                    model.ConfirmPassword = string.Empty;
                    return View(model);
                }
            }

            // Si le modèle n'est pas valide, retourner la vue avec les erreurs de validation
            model.Password = string.Empty;
            model.ConfirmPassword = string.Empty;
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Si le modèle est valide
            if (ModelState.IsValid)
            {
                // Tentative de connexion de l'utilisateur
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Redirection vers la page d'accueil si la connexion est réussie
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    // Si l'utilisateur est verrouillé
                    ModelState.AddModelError(string.Empty, "Votre compte est verrouillé. Veuillez réessayer plus tard.");
                }
                else
                {
               
                    // Vérification si l'email existe
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
             
                        // Si l'utilisateur existe mais que le mot de passe est incorrect
                        ModelState.AddModelError("Password", "Le mot de passe est incorrect.");
                    }
                    else
                    {
                        // Si l'utilisateur n'existe pas
                        ModelState.AddModelError("Email", "Utilisateur introuvable.");
                    }


                }
            }

            // Nettoyage du mot de passe en cas d'erreur pour des raisons de sécurité
            model.Password = string.Empty;

            // Retourne la vue avec les erreurs de validation
            return View(model);
        }


    }

}