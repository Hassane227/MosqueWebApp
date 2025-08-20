using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosqueWebApp.Data;
using MosqueWebApp.Models;

namespace MosqueWebApp.Controllers
{
    public class EvenementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public EvenementController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // Afficher les événements liés à la mosquée de l'imam
        [Authorize(Roles = "Imam")]

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser?.MosqueeId == null)
                return Unauthorized();

            var events = await _context.Evenements
                .Where(e => e.MosqueeId == currentUser.MosqueeId)
                .Include(e => e.Mosquee)
                .ToListAsync();

            return View(events);
        }

        // GET: Create
        [Authorize(Roles = "Imam")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        // POST: Create
        [Authorize(Roles = "Imam")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Evenement evenement, IFormFile? ImageFile)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.MosqueeId == null) return Unauthorized();

            evenement.MosqueeId = currentUser.MosqueeId.Value;

            // Dossier où seront stockées les images
            string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "evenements");
            Directory.CreateDirectory(uploadsFolder);

            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Génère un nom unique pour l'image
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                evenement.Image = "/images/evenements/" + uniqueFileName;
            }
            else
            {
                // Image par défaut si aucune image uploadée
                evenement.Image = "/images/evenements/default-event.jpg";
            }

            _context.Add(evenement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        [Authorize(Roles = "Imam")]
        public async Task<IActionResult> Edit(int? id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var evenement = await _context.Evenements.FindAsync(id);

            if (evenement == null || evenement.MosqueeId != currentUser?.MosqueeId)
                return NotFound();

            return View(evenement);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Evenement evenement, IFormFile? ImageFile)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (id != evenement.Id || currentUser?.MosqueeId != evenement.MosqueeId)
                return Unauthorized();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEvent = await _context.Evenements.AsNoTracking()
                        .FirstOrDefaultAsync(e => e.Id == id);

                    if (existingEvent == null)
                        return NotFound();

                    // Si aucune nouvelle image n’est uploadée → garder l’ancienne
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "evenements");
                        Directory.CreateDirectory(uploadsFolder);

                        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }

                        evenement.Image = "/images/evenements/" + uniqueFileName;
                    }
                    else
                    {
                        // garder l’image existante
                        evenement.Image = existingEvent.Image;
                    }

                    _context.Update(evenement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Evenements.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(evenement);
        }

        // GET: Event/Details/5
        [Authorize(Roles = "Member,Imam")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var eventItem = await _context.Evenements
                .Include(e => e.Mosquee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (eventItem == null)
                return NotFound();

            return View(eventItem);
        }


        // POST: Delete
        [Authorize(Roles = "Imam")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var evenement = await _context.Evenements.FindAsync(id);

            if (evenement == null || evenement.MosqueeId != currentUser?.MosqueeId)
                return NotFound();

            _context.Evenements.Remove(evenement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
