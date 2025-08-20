using System.ComponentModel.DataAnnotations;

namespace MosqueWebApp.Models
{
    public class BesoinMateriel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Nom { get; set; }

        public string? Description { get; set; }

        public DateTime DateDemande { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }

        // FK
        public int MosqueeId { get; set; }
        public Mosquee? Mosquee { get; set; }
    }

}
