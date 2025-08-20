using System.ComponentModel.DataAnnotations;

namespace MosqueWebApp.Models
{
    public class ContenuReligieux
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Titre { get; set; }

        [MaxLength(100)]
        public string? Type { get; set; }

        public string? ContenuTexte { get; set; }
        public string? UrlMedia { get; set; }

        public DateTime? DatePublication { get; set; }

        [MaxLength(20)]
        public string? Langue { get; set; }

        // FK vers ApplicationUser (auteur)
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }

}
