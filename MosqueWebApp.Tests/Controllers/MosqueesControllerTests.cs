using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MosqueWebApp.Controllers;
using MosqueWebApp.Data;
using MosqueWebApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;



namespace MosqueWebApp.Tests
{
    public class MosqueesControllerTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "MosqueesDb_" + Guid.NewGuid())
                .Options;
            return new ApplicationDbContext(options);
        }

        private Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);
        }

        private Mock<IWebHostEnvironment> GetMockEnv()
        {
            var env = new Mock<IWebHostEnvironment>();
            env.Setup(e => e.WebRootPath).Returns(Path.GetTempPath()); // Utilise un dossier temporaire
            return env;
        }

        [Fact]
        public async Task Index_ReturnsAllMosquees_WhenNoSearch()
        {
            // Arrange
            var db = GetDbContext();
            db.Mosquees.Add(new Mosquee { Id = 1, Nom = "Mosquée A", Ville = "Casablanca" });
            db.Mosquees.Add(new Mosquee { Id = 2, Nom = "Mosquée B", Ville = "Rabat" });
            db.SaveChanges();

            var controller = new MosqueesController(db, GetMockUserManager().Object, GetMockEnv().Object);

            // Act
            var result = await controller.Index(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Mosquee>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task Index_FiltersBySearchString()
        {
            // Arrange
            var db = GetDbContext();
            db.Mosquees.Add(new Mosquee { Id = 1, Nom = "Mosquée Hassan II", Ville = "Casablanca" });
            db.Mosquees.Add(new Mosquee { Id = 2, Nom = "Mosquée Agadir", Ville = "Agadir" });
            db.SaveChanges();

            var controller = new MosqueesController(db, GetMockUserManager().Object, GetMockEnv().Object);

            // Act
            var result = await controller.Index("Agadir");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Mosquee>>(viewResult.Model);
            Assert.Single(model);
            Assert.Equal("Mosquée Agadir", model.First().Nom);
        }

        [Fact]
        public async Task Create_AddsMosquee_AndRedirects()
        {
            // Arrange
            var db = GetDbContext();
            var controller = new MosqueesController(db, GetMockUserManager().Object, GetMockEnv().Object);

            var mosquee = new Mosquee { Id = 1, Nom = "Mosquée Test", Ville = "Marrakech" };

            // Act
            var result = await controller.Create(mosquee, null);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);

            Assert.Equal(1, db.Mosquees.Count());
            Assert.Equal("Mosquée Test", db.Mosquees.First().Nom);
        }

        [Fact]
        public async Task Delete_RemovesMosquee_AndRedirects()
        {
            // Arrange
            var db = GetDbContext();
            db.Mosquees.Add(new Mosquee { Id = 1, Nom = "Mosquée à supprimer", Ville = "Tanger" });
            db.SaveChanges();

            var controller = new MosqueesController(db, GetMockUserManager().Object, GetMockEnv().Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Empty(db.Mosquees);
        }
    }
}
