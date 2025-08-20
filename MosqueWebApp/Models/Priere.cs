using System.ComponentModel.DataAnnotations;

namespace MosqueWebApp.Models
{
    public class Priere
    {
        public int Id { get; set; }

        [Required]
        public int MosqueeId { get; set; }
        public Mosquee? Mosquee { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? HeureFajr { get; set; }
        public TimeSpan? HeureDuhr { get; set; }
        public TimeSpan? HeureAsr { get; set; }
        public TimeSpan? HeureMaghrib { get; set; }
        public TimeSpan? HeureIsha { get; set; }
    }

}
