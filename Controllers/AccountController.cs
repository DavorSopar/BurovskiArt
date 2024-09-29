using ArtistPortfolio.Models.DTO;
using ArtistPortfolio.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArtistPortfolio.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            UserRegistrationDTO model = new UserRegistrationDTO();
            {
                RoleList = _roleManager.Roles.Select(x=>x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                });
            };
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationDTO request)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await _userManager.FindByEmailAsync(request.Email);
                if (userCheck == null)
                {
                    var user = new ApplicationUser
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        UserName = request.Email,
                        NormalizedUserName = request.Email,
                        Email = request.Email,
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(user, request.Password);
                    if (result.Succeeded)
                    {
                        // Directly assign the 'Customer' role to the new user
                        await _userManager.AddToRoleAsync(user, SD.Role_Customer);

                        return RedirectToAction("Login");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("message", error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists.");
                }
            }

            return View(request);
        }
        public string? Role;

        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }

        //GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            UserLoginDTO model = new UserLoginDTO();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // Get the currently logged-in user by email
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var roles = await _userManager.GetRolesAsync(user);

                    foreach (var role in roles)
                    {
                        Console.WriteLine($"User role: {role}");
                    }

                    // Force sign out and re-sign in to refresh claims
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Check if the user is in the "Admin" role
                    if (await _userManager.IsInRoleAsync(user, SD.Role_Admin))
                    {
                        // Redirect to the ImageDashboard for admin
                        return RedirectToAction("Index", "ImageDashboard");
                    }
                    else if (await _userManager.IsInRoleAsync(user, SD.Role_Customer))
                    {
                        // Redirect to the Home index for customer
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            ViewBag.Message = "You do not have the necessary permissions to access this page. Only administrators can access this section.";
            return View();
        }
    }
}