using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MosqueWebApp.Data;
using MosqueWebApp.Models;

namespace MosqueWebApp.Controllers
{
    [Authorize(Roles = "Imam")]
    public class PriereController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PriereController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Afficher la liste des horaires pour la mosquée de l'imam
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.MosqueeId == null) return Unauthorized();

            var prieres = await _context.Prieres
                .Where(p => p.MosqueeId == currentUser.MosqueeId)
                .ToListAsync();

            return View(prieres);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Priere priere)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.MosqueeId == null) return Unauthorized();

            priere.MosqueeId = currentUser.MosqueeId.Value;
            _context.Add(priere);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.MosqueeId == null) return Unauthorized();

            var priere = await _context.Prieres
                .FirstOrDefaultAsync(p => p.Id == id && p.MosqueeId == user.MosqueeId);

            if (priere == null) return NotFound();

            return View(priere);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Priere priere)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.MosqueeId == null) return Unauthorized();

            var priereDb = await _context.Prieres
                .FirstOrDefaultAsync(p => p.Id == id && p.MosqueeId == user.MosqueeId);

            if (priereDb == null) return NotFound();

            if (ModelState.IsValid)
            {
                priereDb.Date = priere.Date;
                priereDb.HeureFajr = priere.HeureFajr;
                priereDb.HeureDuhr = priere.HeureDuhr;
                priereDb.HeureAsr = priere.HeureAsr;
                priereDb.HeureMaghrib = priere.HeureMaghrib;
                priereDb.HeureIsha = priere.HeureIsha;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(priere);
        }

        // POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.MosqueeId == null) return Unauthorized();

            var priere = await _context.Prieres
                .FirstOrDefaultAsync(p => p.Id == id && p.MosqueeId == currentUser.MosqueeId);

            if (priere == null) return NotFound();

            _context.Prieres.Remove(priere);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
