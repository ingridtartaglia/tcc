using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TrainingSystem.Controllers;
using TrainingSystem.Data;
using TrainingSystem.Models;
using TrainingSystemTests;
using TrainingSystemTests.Utils;
using Xunit;

namespace TrainingSystem.Tests.ControllerTests
{
    public class CoursesControllerTests : IDisposable
    {
        private readonly TrainingSystemContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CoursesControllerTests()
        {
            var services = new ServiceCollection();
            services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<TrainingSystemContext>(options => options.UseInMemoryDatabase("TrainingSystem"));
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<TrainingSystemContext>();
            var context = new DefaultHttpContext();
            context.Features.Set<IHttpAuthenticationFeature>(new HttpAuthenticationFeature());
            services.AddSingleton<IHttpContextAccessor>(h => new HttpContextAccessor { HttpContext = context });
            var serviceProvider = services.BuildServiceProvider();
            _dbContext = serviceProvider.GetRequiredService<TrainingSystemContext>();
            _userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        }

        [Fact]
        public void GetCourses()
        {
            // Arrange
            var appUser = new AppUser()
            {
                Id = "1",
                Email = "admin@email.com",
                UserName = "admin@email.com"
            };
            var course1 = new Course() {
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var course2 = new Course() {
                Name = "Curso 2",
                Category = "Asp.net",
                Instructor = "Fulano"
            };
            _userManager.CreateAsync(appUser, "Teste!23").Wait();
            _dbContext.Course.Add(course1);
            _dbContext.Course.Add(course2);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            
            var controller = new CoursesController(_dbContext, _userManager) {
                ObjectValidator = objectValidator.Object
            };
            Helpers.SetupUser(controller, "admin@email.com");

            // Act
            var response = controller.GetCourses();

            // Assert
            response.Should().HaveCount(2);
        }

        [Fact]
        public void GetCourse()
        {
            // Arrange
            var appUser = new AppUser()
            {
                Id = "1",
                Email = "admin@email.com",
                UserName = "admin@email.com"
            };
            var course1 = new Course() {
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            _roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
            _userManager.CreateAsync(appUser, "Teste!23").Wait();
            _userManager.AddToRoleAsync(appUser, "Admin").Wait();
            _dbContext.Course.Add(course1);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new CoursesController(_dbContext, _userManager) {
                ObjectValidator = objectValidator.Object
            };
            Helpers.SetupUser(controller, "admin@email.com");
            
            // Act
            var response = (OkObjectResult)controller.GetCourse(1).Result;

            // Assert
            ((Course)response.Value).CourseId.Should().Be(1);
        }

        [Fact]
        public void GetNotFoundCourse()
        {
            // Arrange
            var course1 = new Course() {
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            _dbContext.Course.Add(course1);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new CoursesController(_dbContext, _userManager) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (NotFoundResult)controller.GetCourse(50).Result;

            // Assert
            response.StatusCode.Should().Be(404);
        }

        [Fact]
        public void PostCourse()
        {
            // Arrange
            var courseToAdd = new Course() {
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new CoursesController(_dbContext, _userManager) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (CreatedAtActionResult)controller.PostCourse(courseToAdd).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.Created, response.StatusCode);
            ((Course)response.Value).CourseId.Should().NotBe(0);
            ((Course)response.Value).Name.Should().Be("Curso 1");
        }

        [Fact]
        public void CreateInvalidCourse()
        {
            // Arrange
            var courseToAdd = new Course() {
                Name = "",
                Category = "Outros",
                Instructor = "Fulano"
            };

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new CoursesController(_dbContext, _userManager) {
                ObjectValidator = objectValidator.Object
            };
            controller.ModelState.AddModelError("Name", "The Name field is required");

            // Act
            var response = (BadRequestObjectResult)controller.PostCourse(courseToAdd).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            ((SerializableError)response.Value).ContainsKey("Name").Should().BeTrue();
        }

        [Fact]
        public void DeleteCourse()
        {
            // Arrange
            var course1 = new Course() {
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            _dbContext.Course.Add(course1);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new CoursesController(_dbContext, _userManager) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (OkObjectResult)controller.DeleteCourse(1).Result;

            // Assert
            response.StatusCode.Should().Be(200);
            _dbContext.Course.Find(1).Should().BeNull();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
