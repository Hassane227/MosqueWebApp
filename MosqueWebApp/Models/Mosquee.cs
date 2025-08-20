using Humanizer;
using System.ComponentModel.DataAnnotations;

namespace MosqueWebApp.Models
{
    public class Mosquee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Nom { get; set; }
        [MaxLength(200)]
        public string? Adresse { get; set; }
        public string? Ville { get; set; }
        public string? Region { get; set; }
        public string? Contact { get; set; }
        public string? Image { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
        public string? AdminMosqueeId { get; set; }
        public ApplicationUser? AdminMosquee { get; set; }

        public string? ImamId { get; set; }
        public ApplicationUser? Imam { get; set; }

        // Navigation
        public ICollection<Evenement>? Evenements { get; set; }
        public ICollection<Don>? Dons { get; set; }
        public ICollection<BesoinMateriel>? BesoinsMateriels { get; set; }
        public ICollection<Priere>? PriereHoraires { get; set; }
        public ICollection<ApplicationUser>? Membres { get; set; }

    }

}
