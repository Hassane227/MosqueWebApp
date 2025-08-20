using System.ComponentModel.DataAnnotations;

namespace MosqueWebApp.Models
{
    public class Evenement
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string? Titre { get; set; }

        public string? Description { get; set; }
        public string? Image { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }

        // FK

        public int MosqueeId { get; set; }
        public Mosquee? Mosquee { get; set; }

        // Navigation
        public ICollection<InscriptionEvenement>? Inscriptions { get; set; }
    }

}
