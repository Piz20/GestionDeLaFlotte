using Gestion_de_la_flotte.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            // Vérifier si l'utilisateur est déjà authentifié
            if (User.Identity.IsAuthenticated)
            {
                // Rediriger vers le tableau de bord si l'utilisateur est déjà authentifié
                return RedirectToAction("AdminDashBoard", "Admin");
            }

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
                        return RedirectToAction("AdminDashBoard", "Admin");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    model.Password = string.Empty;
                    model.ConfirmPassword = string.Empty;
                    return View(model);
                }
            }

            model.Password = string.Empty;
            model.ConfirmPassword = string.Empty;
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Vérifier si l'utilisateur est déjà authentifié
            if (User.Identity.IsAuthenticated)
            {
                // Rediriger vers le tableau de bord si l'utilisateur est déjà authentifié
                return RedirectToAction("AdminDashBoard", "Admin");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("AdminDashBoard", "Admin");
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Votre compte est verrouillé. Veuillez réessayer plus tard.");
                }
                else
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        ModelState.AddModelError("Password", "Le mot de passe est incorrect.");
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Utilisateur introuvable.");
                    }
                }
            }

            model.Password = string.Empty;
            return View(model);
        }
    }
}
