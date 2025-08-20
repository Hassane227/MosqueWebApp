using Microsoft.AspNetCore.Mvc;
using MosqueWebApp.Data;
using MosqueWebApp.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;


namespace MosqueWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var mosquees = await _context.Mosquees.ToListAsync();
            var evenements = await _context.Evenements
                .Include(e => e.Mosquee)
                .OrderByDescending(e => e.DateDebut)
                .ToListAsync();

            ViewBag.Evenements = evenements;
            return View(mosquees);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
