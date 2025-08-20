using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MosqueWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string? FullName { get; set; }
        [ForeignKey("Mosquee")]
        public int? MosqueeId { get; set; }
        public Mosquee? Mosquee { get; set; }

        // Publications religieuses
        // Navigation properties
        public ICollection<ContenuReligieux>? ContenusPublies { get; set; }
        public ICollection<Don>? Dons { get; set; }
        public ICollection<InscriptionEvenement>? InscriptionsEvenements { get; set; }

    }
}
