namespace MosqueWebApp.Models
{
    public class InscriptionEvenement
    {
        public int Id { get; set; }

        // FK vers ApplicationUser
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        // FK vers Evenement
        public int EvenementId { get; set; }
        public Evenement? Evenement { get; set; }
    }

}
