using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using MosqueWebApp.Controllers;
using MosqueWebApp.Data;
using MosqueWebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;


namespace MosqueWebApp.Tests.Controllers
{
    public class EvenementControllerTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // chaque test => nouvelle DB
                .Options;

            return new ApplicationDbContext(options);
        }

        private Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private Mock<IWebHostEnvironment> GetMockEnv()
        {
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.WebRootPath).Returns(Path.GetTempPath()); // dossier temporaire
            return mockEnv;
        }

        private ClaimsPrincipal FakeUser(string userId)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            return new ClaimsPrincipal(identity);
        }

        [Fact]
        public async Task Index_ReturnsEventsOfCurrentUserMosquee()
        {
            // Arrange
            var db = GetDbContext();
            var userManager = GetMockUserManager();
            var env = GetMockEnv();

            var user = new ApplicationUser { Id = "user1", MosqueeId = 1 };
            userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            db.Mosquees.Add(new Mosquee { Id = 1, Nom = "MosqueeTest" });
            db.Evenements.Add(new Evenement { Id = 1, Titre = "Event1", MosqueeId = 1 });
            db.Evenements.Add(new Evenement { Id = 2, Titre = "Event2", MosqueeId = 2 });
            db.SaveChanges();

            var controller = new EvenementController(db, userManager.Object, env.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = FakeUser("user1") }
                }
            };

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Evenement>>(viewResult.Model);
            Assert.Single(model); // seulement Event1
        }

        [Fact]
        public async Task Create_AddsEventAndRedirects()
        {
            // Arrange
            var db = GetDbContext();
            var userManager = GetMockUserManager();
            var env = GetMockEnv();

            var user = new ApplicationUser { Id = "user1", MosqueeId = 1 };
            userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var controller = new EvenementController(db, userManager.Object, env.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = FakeUser("user1") }
                }
            };

            var newEvent = new Evenement { Titre = "Nouvel Event" };

            // Act
            var result = await controller.Create(newEvent, null);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Single(db.Evenements); // Event enregistré
        }


        [Fact]
        public async Task Delete_RemovesEventAndRedirects()
        {
            // Arrange
            var db = GetDbContext();
            var userManager = GetMockUserManager();
            var env = GetMockEnv();

            var user = new ApplicationUser { Id = "user1", MosqueeId = 1 };
            userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            db.Evenements.Add(new Evenement { Id = 1, Titre = "ToDelete", MosqueeId = 1 });
            db.SaveChanges();

            var controller = new EvenementController(db, userManager.Object, env.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = FakeUser("user1") }
                }
            };

            // Act
            var result = await controller.Delete(1);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Empty(db.Evenements);
        }
    }
}
