using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ArtistPortfolio.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace ArtistPortfolio.Controllers
{
    [Authorize]
    //[Authorize(Roles = "Admin")]
    public class AdminPanelController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminPanelController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: /AdminPanel/Users
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync(); // Retrieve all users
            return View(users);
        }

        // GET: /AdminPanel/EditUser/{id}
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // Get roles assigned to the user
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = roles; // Send roles to view

            return View(user); // Return the user object to the view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(ApplicationUser user, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                // Find the existing user
                var existingUser = await _userManager.FindByIdAsync(user.Id);
                if (existingUser == null) return NotFound();

                // Update user properties
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;

                // Update user details in the database
                var result = await _userManager.UpdateAsync(existingUser);
                if (result.Succeeded)
                {
                    // Get the current roles of the user
                    var currentRoles = await _userManager.GetRolesAsync(existingUser);

                    // Remove all current roles
                    await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);

                    // Add selected roles
                    await _userManager.AddToRolesAsync(existingUser, selectedRoles);

                    return RedirectToAction("Users"); // Redirect back to the user list
                }
            }

            return View(user); // Return the user back to the view on error
        }

    }
}
