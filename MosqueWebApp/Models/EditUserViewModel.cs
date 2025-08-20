using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MosqueWebApp.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? FullName { get; set; }

        public List<RoleSelectionViewModel> Roles { get; set; } = new();


        // Pour la modification du mot de passe (optionnel)
        [Display(Name = "Mosque")]
        public int? MosqueId { get; set; }

        public List<SelectListItem>? Mosques { get; set; }
    }

    public class RoleSelectionViewModel
    {
        public string RoleId { get; set; }

        public string? RoleName { get; set; }
        public bool Selected { get; set; }
    }
}
