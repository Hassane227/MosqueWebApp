using System.ComponentModel.DataAnnotations;

namespace MosqueWebApp.Models
{
    public class Don
    {
        public int Id { get; set; }

        [Required]
        public int MosqueeId { get; set; }
        public Mosquee? Mosquee { get; set; }

        [Required]
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public bool Anonyme { get; set; }

        [MaxLength(50)]
        public string? MethodePaiement { get; set; }

        [Required]
        public decimal Montant { get; set; }

        [MaxLength(100)]
        public string? Type { get; set; }
    }

}
