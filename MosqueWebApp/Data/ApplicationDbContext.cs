using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MosqueWebApp.Models;

namespace MosqueWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Mosquee> Mosquees { get; set; }
        public DbSet<Evenement> Evenements { get; set; }
        public DbSet<InscriptionEvenement> InscriptionsEvenements { get; set; }
        public DbSet<Don> Dons { get; set; }
        public DbSet<ContenuReligieux> ContenusReligieux { get; set; }
        public DbSet<BesoinMateriel> BesoinsMateriels { get; set; }
        public DbSet<Priere> Prieres { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Don>()
                .Property(d => d.Montant)
                .HasPrecision(18, 2);

            // InscriptionEvenement
            builder.Entity<InscriptionEvenement>()
                .HasOne(i => i.User)
                .WithMany(u => u.InscriptionsEvenements)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<InscriptionEvenement>()
                .HasOne(i => i.Evenement)
                .WithMany(e => e.Inscriptions)
                .HasForeignKey(i => i.EvenementId)
                .OnDelete(DeleteBehavior.Cascade);

            // ContenuReligieux
            builder.Entity<ContenuReligieux>()
                .HasOne(c => c.User)
                .WithMany(u => u.ContenusPublies)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Don
            builder.Entity<Don>()
                .HasOne(d => d.User)
                .WithMany(u => u.Dons)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Don>()
                .HasOne(d => d.Mosquee)
                .WithMany(m => m.Dons)
                .HasForeignKey(d => d.MosqueeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Evenement
            builder.Entity<Evenement>()
                .HasOne(e => e.Mosquee)
                .WithMany(m => m.Evenements)
                .HasForeignKey(e => e.MosqueeId)
                .OnDelete(DeleteBehavior.Cascade);

            // BesoinMateriel
            builder.Entity<BesoinMateriel>()
                .HasOne(b => b.Mosquee)
                .WithMany(m => m.BesoinsMateriels)
                .HasForeignKey(b => b.MosqueeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Priere
            builder.Entity<Priere>()
                .HasOne(p => p.Mosquee)
                .WithMany(m => m.PriereHoraires)
                .HasForeignKey(p => p.MosqueeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
            .HasOne(u => u.Mosquee)
            .WithMany(m => m.Membres)
            .HasForeignKey(u => u.MosqueeId)
            .OnDelete(DeleteBehavior.SetNull);
            // 

            // Mosquee → AdminMosquee
            builder.Entity<Mosquee>()
                .HasOne(m => m.AdminMosquee)
                .WithMany()
                .HasForeignKey(m => m.AdminMosqueeId)
                .OnDelete(DeleteBehavior.SetNull);

            // Mosquee → Imam
            builder.Entity<Mosquee>()
            .HasOne(m => m.Imam)
            .WithMany()
            .HasForeignKey(m => m.ImamId)
            .OnDelete(DeleteBehavior.Restrict); // ou NoAction

        }
    }
}
