using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MosqueWebApp.Data;
using MosqueWebApp.Models;

namespace MosqueWebApp.Controllers
{
    public class MosqueesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MosqueesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Mosquees/Index

        public async Task<IActionResult> Index(string searchString)
        {
            var usersQuery = _context.Mosquees;

            if (!string.IsNullOrEmpty(searchString))
            {
                // Filtrer par nom complet ou email
                var filteredUsers = await usersQuery
                    .Where(u => u.Ville.Contains(searchString) || u.Nom.Contains(searchString))
                    .ToListAsync();

                return View(filteredUsers);
            }

            var Mosquee = await usersQuery.ToListAsync();
            return View(Mosquee);
        }

        // GET: Mosquees/Create
        public async Task<IActionResult> Create()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var imams = await _userManager.GetUsersInRoleAsync("Imam");

            ViewData["Admins"] = new SelectList(admins, "Id", "FullName");
            ViewData["Imams"] = new SelectList(imams, "Id", "FullName");

            return View();
        }


        // POST: Mosquees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Mosquee mosquee, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "mosquees");
                    Directory.CreateDirectory(uploadsFolder); // crée le dossier s'il n'existe pas

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    mosquee.Image = "/images/mosquees/" + uniqueFileName;
                }

                _context.Add(mosquee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var imams = await _userManager.GetUsersInRoleAsync("Imam");

            ViewData["Admins"] = new SelectList(admins, "Id", "FullName", mosquee.AdminMosqueeId);
            ViewData["Imams"] = new SelectList(imams, "Id", "FullName", mosquee.ImamId);


            return View(mosquee);
        }

        // GET: Mosquees/Edit/5
        // GET: Mosquees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mosquee = await _context.Mosquees.FindAsync(id);
            if (mosquee == null)
            {
                return NotFound();
            }

            // Charger les admins et imams
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var imams = await _userManager.GetUsersInRoleAsync("Imam");

            ViewData["Admins"] = new SelectList(admins, "Id", "FullName", mosquee.AdminMosqueeId);
            ViewData["Imams"] = new SelectList(imams, "Id", "FullName", mosquee.ImamId);

            return View(mosquee);
        }


        // POST: Mosquees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Mosquee mosquee, IFormFile? ImageFile)
        {
            if (id != mosquee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // ✅ Nouvelle image fournie
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "mosquees");
                        Directory.CreateDirectory(uploadsFolder);

                        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(fileStream);
                        }

                        // ✅ Supprimer l'ancienne image si elle existe
                        if (!string.IsNullOrEmpty(mosquee.Image))
                        {
                            string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, mosquee.Image.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                                System.IO.File.Delete(oldImagePath);
                        }

                        mosquee.Image = "/images/mosquees/" + uniqueFileName;
                    }
                    else
                    {
                        // ✅ Aucune nouvelle image : on récupère l'image actuelle depuis la BDD
                        var existingMosquee = await _context.Mosquees.AsNoTracking()
                            .FirstOrDefaultAsync(m => m.Id == id);

                        if (existingMosquee != null)
                        {
                            mosquee.Image = existingMosquee.Image;
                        }
                    }

                    _context.Update(mosquee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MosqueeExists(mosquee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var imams = await _userManager.GetUsersInRoleAsync("Imam");

            ViewData["Admins"] = new SelectList(admins, "Id", "FullName", mosquee.AdminMosqueeId);
            ViewData["Imams"] = new SelectList(imams, "Id", "FullName", mosquee.ImamId);

            return View(mosquee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var mosquee = await _context.Mosquees.FindAsync(id);
            if (mosquee != null)
            {
                _context.Mosquees.Remove(mosquee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Vérifie si la mosquée existe
        private bool MosqueeExists(int id)
        {
            return _context.Mosquees.Any(e => e.Id == id);
        }


    }
}
