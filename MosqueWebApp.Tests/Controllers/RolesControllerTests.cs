using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MosqueWebApp.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MosqueWebApp.Tests.Controllers
{
    public class RolesControllerTests
    {
        // Méthode utilitaire pour créer un mock de RoleManager
        private Mock<RoleManager<IdentityRole>> MockRoleManager()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();
            return new Mock<RoleManager<IdentityRole>>(
                store.Object, null, null, null, null);
        }


        [Fact]
        public async Task Create_ValidRoleName_RedirectsToIndex()
        {
            // Arrange
            var mockRoleManager = MockRoleManager();
            mockRoleManager
                .Setup(rm => rm.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            var controller = new RolesController(mockRoleManager.Object);

            // Act
            var result = await controller.Create("Manager");

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task Edit_Post_ReturnsRedirect_WhenUpdateSuccessful()
        {
            // Arrange
            var roleId = "1";
            var newName = "Admin";

            var mockRoleManager = MockRoleManager();
            mockRoleManager.Setup(r => r.FindByIdAsync(roleId))
                .ReturnsAsync(new IdentityRole { Id = roleId, Name = "OldName" });

            mockRoleManager.Setup(r => r.UpdateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            var controller = new RolesController(mockRoleManager.Object);

            // Act
            var result = await controller.Edit(roleId, newName);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task Edit_Post_ReturnsView_WhenNameIsEmpty()
        {
            // Arrange
            var roleId = "1";

            var mockRoleManager = MockRoleManager();
            var role = new IdentityRole { Id = roleId, Name = "OldName" };

            mockRoleManager.Setup(r => r.FindByIdAsync(roleId))
                .ReturnsAsync(role);

            var controller = new RolesController(mockRoleManager.Object);

            // Act
            var result = await controller.Edit(roleId, "");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(role, viewResult.Model);
            Assert.False(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task Edit_Post_ReturnsView_WhenUpdateFails()
        {
            // Arrange
            var roleId = "1";
            var role = new IdentityRole { Id = roleId, Name = "OldName" };
            var newName = "Admin";

            var mockRoleManager = MockRoleManager();

            mockRoleManager.Setup(r => r.FindByIdAsync(roleId))
                .ReturnsAsync(role);

            mockRoleManager.Setup(r => r.UpdateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Erreur de mise à jour" }));

            var controller = new RolesController(mockRoleManager.Object);

            // Act
            var result = await controller.Edit(roleId, newName);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Same(role, viewResult.Model);
            Assert.False(controller.ModelState.IsValid);
        }
    }
}
