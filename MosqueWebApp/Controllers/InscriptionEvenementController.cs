using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosqueWebApp.Data;
using MosqueWebApp.Models;

namespace MosqueWebApp.Controllers
{
    [Authorize] // 🔒 il faut être connecté pour toutes les actions
    public class InscriptionEvenementController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public InscriptionEvenementController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ Inscrire un utilisateur connecté à un événement
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Inscrire(int evenementId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // Vérifier si l’utilisateur est déjà inscrit
            bool dejaInscrit = await _context.InscriptionsEvenements
                .AnyAsync(i => i.UserId == user.Id && i.EvenementId == evenementId);

            if (dejaInscrit)
            {
                TempData["Message"] = "Vous êtes déjà inscrit à cet événement.";
                return RedirectToAction("Details", "Evenement", new { id = evenementId });
            }

            var inscription = new InscriptionEvenement
            {
                UserId = user.Id,
                EvenementId = evenementId
            };

            _context.InscriptionsEvenements.Add(inscription);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Inscription réussie 🎉";
            return RedirectToAction("Details", "Evenement", new { id = evenementId });
        }

        // ✅ Liste des inscriptions d’un membre
        public async Task<IActionResult> MesInscriptions()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var inscriptions = await _context.InscriptionsEvenements
                .Include(i => i.Evenement)
                .ThenInclude(e => e.Mosquee)
                .Where(i => i.UserId == user.Id)
                .ToListAsync();

            return View(inscriptions);
        }

        // ✅ Liste des participants d’un événement (pour l’Imam de la mosquée uniquement)
        [Authorize(Roles = "Imam")]
        public async Task<IActionResult> Participants(int evenementId)
        {
            var user = await _userManager.GetUserAsync(User);

            var evenement = await _context.Evenements
                .Include(e => e.Mosquee)
                .ThenInclude(m => m.Imam) // charge l'objet ApplicationUser lié à l'imam

                .FirstOrDefaultAsync(e => e.Id == evenementId);

            if (evenement == null) return NotFound();

            // Vérifier si l’imam est bien celui de la mosquée
            if (evenement.Mosquee.ImamId != user.Id)
            {
                return Forbid();
            }

            var participants = await _context.InscriptionsEvenements
                .Include(i => i.User)
                .Where(i => i.EvenementId == evenementId)
                .ToListAsync();

            return View(participants);
        }

        // ✅ Annuler son inscription
        [HttpPost]
        public async Task<IActionResult> Annuler(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var inscription = await _context.InscriptionsEvenements
                .FirstOrDefaultAsync(i => i.Id == id && i.UserId == user.Id);

            if (inscription == null) return NotFound();

            _context.InscriptionsEvenements.Remove(inscription);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Votre inscription a été annulée.";
            return RedirectToAction("MesInscriptions");
        }
    }
}
