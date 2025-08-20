using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MosqueWebApp.Data;
using MosqueWebApp.Models;
using Microsoft.EntityFrameworkCore;


namespace MosqueWebApp.Controllers
{
    public class ImamController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public ImamController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;

        }

        public async Task<IActionResult> Index(string searchString)
        {
            // Récupérer l'utilisateur connecté (l'imam)
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null || currentUser.MosqueeId == null)
            {
                return Unauthorized(); // ou RedirectToAction("Login", "Account");
            }

            // Récupérer l'ID de la mosquée de l'imam
            var imamMosqueeId = currentUser.MosqueeId;

            // Requête des utilisateurs liés à la même mosquée que l'imam
            var usersQuery = _userManager.Users
                .Include(u => u.Mosquee)
                .Where(u => u.MosqueeId == imamMosqueeId);

            // Filtrage par recherche
            if (!string.IsNullOrEmpty(searchString))
            {
                usersQuery = usersQuery.Where(u =>
                    u.FullName.Contains(searchString) || u.Email.Contains(searchString));
            }

            var users = await usersQuery.ToListAsync();
            return View(users);
        }



        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.Mosquee)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);
            var mosques = await _context.Mosquees.ToListAsync();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                MosqueId = user.MosqueeId,
                Mosques = mosques.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Nom
                }).ToList(),
                Roles = roles.Select(role => new RoleSelectionViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Selected = userRoles.Contains(role.Name)
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Mosques = await _context.Mosquees
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nom })
                    .ToListAsync();
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            // Mise à jour des propriétés utilisateur
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.MosqueeId = model.MosqueId;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError("", error.Description);

                model.Mosques = await _context.Mosquees
                    .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Nom })
                    .ToListAsync();

                return View(model);
            }

            // Gestion des rôles : suppression complète puis ajout des rôles sélectionnés
            var currentRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = model.Roles.Where(r => r.Selected).Select(r => r.RoleName).ToList();

            if (currentRoles.Count > 0)
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (selectedRoles.Count > 0)
                await _userManager.AddToRolesAsync(user, selectedRoles);

            return RedirectToAction("Index");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Erreur lors de la suppression de l'utilisateur.");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
