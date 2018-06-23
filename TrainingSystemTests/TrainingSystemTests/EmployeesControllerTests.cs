using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Net;
using TrainingSystem.Controllers;
using TrainingSystem.Data;
using TrainingSystem.Models;
using TrainingSystem.ViewModels;
using TrainingSystemTests;
using Xunit;

namespace TrainingSystem.Tests.ControllerTests
{
    public class EmployeesControllerTests
    {
        private readonly TrainingSystemContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeesControllerTests()
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
        public void GetEmployees()
        {
            // Arrange
            var appUser = new AppUser()
            {
                Id = "1",
                Email = "estagiario@email.com",
                UserName = "estagiario@email.com"
            };
            var employee1 = new Employee()
            {
                AppUserId = "1",
                Occupation = "Estagiário"
            };
            var employee2 = new Employee()
            {
                AppUserId = "1",
                Occupation = "Analista"
            };
            _userManager.CreateAsync(appUser, "Teste!23").Wait();
            _dbContext.Employee.Add(employee1);
            _dbContext.Employee.Add(employee2);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));

            var controller = new EmployeesController(_dbContext, _userManager)
            {
                ObjectValidator = objectValidator.Object
            };
            Helpers.SetupUser(controller, "admin@email.com");

            // Act
            var response = controller.GetEmployees();

            // Assert
            response.Should().HaveCount(2);
        }

        [Fact]
        public void GetEmployee()
        {
            // Arrange
            var appUser = new AppUser()
            {
                Id = "1",
                Email = "estagiario@email.com",
                UserName = "estagiario@email.com"
            };
            var employee = new Employee()
            {
                AppUserId = "1",
                Occupation = "Estagiário"
            };
            _userManager.CreateAsync(appUser, "Teste!23").Wait();
            _dbContext.Employee.Add(employee);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new EmployeesController(_dbContext, _userManager)
            {
                ObjectValidator = objectValidator.Object
            };
            Helpers.SetupUser(controller, "estagiario@email.com");

            // Act
            var response = (OkObjectResult)controller.GetEmployee(1).Result;

            // Assert
            ((Employee)response.Value).EmployeeId.Should().Be(1);
        }

        [Fact]
        public void GetNotFoundEmployee()
        {
            // Arrange
            var appUser = new AppUser()
            {
                Id = "1",
                Email = "estagiario@email.com",
                UserName = "estagiario@email.com"
            };
            var employee = new Employee()
            {
                AppUserId = "1",
                Occupation = "Estagiário"
            };
            _userManager.CreateAsync(appUser, "Teste!23").Wait();
            _dbContext.Employee.Add(employee);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new EmployeesController(_dbContext, _userManager)
            {
                ObjectValidator = objectValidator.Object
            };
            Helpers.SetupUser(controller, "estagiario@email.com");

            // Act
            var response = (NotFoundResult)controller.GetEmployee(50).Result;

            // Assert
            response.StatusCode.Should().Be(404);
        }

        [Fact]
        public void PostEmployee()
        {
            // Arrange
            var vm = new RegisterViewModel()
            {
                Email = "estagiario@email.com",
                Occupation = "Estagiário",
                FirstName = "Fulano",
                LastName = "de Tal",
                Password = "Teste!23"
            };
            _roleManager.CreateAsync(new IdentityRole("Employee")).Wait();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new EmployeesController(_dbContext, _userManager)
            {
                ObjectValidator = objectValidator.Object
            };
            Helpers.SetupUser(controller, "admin@email.com");

            // Act
            var response = (CreatedAtActionResult)controller.PostEmployee(vm).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.Created, response.StatusCode);
            ((Employee)response.Value).EmployeeId.Should().NotBe(0);
            ((Employee)response.Value).Occupation.Should().Be("Estagiário");
        }

        [Fact]
        public void CreateInvalidEmployee()
        {            
            // Arrange
            var vm = new RegisterViewModel()
            {
                Email = "estagiario@email.com",
                Occupation = "",
                FirstName = "Fulano",
                LastName = "de Tal",
                Password = "Teste!23"
            };
            _roleManager.CreateAsync(new IdentityRole("Employee")).Wait();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new EmployeesController(_dbContext, _userManager)
            {
                ObjectValidator = objectValidator.Object
            };
            Helpers.SetupUser(controller, "admin@email.com");
            controller.ModelState.AddModelError("Occupation", "Ocupação obrigatória.");

            // Act
            var response = (BadRequestObjectResult)controller.PostEmployee(vm).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            ((SerializableError)response.Value).ContainsKey("Occupation").Should().BeTrue();
        }

        [Fact]
        public void CreateExistentEmployee()
        {
            // Arrange
            var appUser = new AppUser()
            {
                Id = "1",
                Email = "estagiario@email.com",
                UserName = "estagiario@email.com",
                FirstName = "Fulano",
                LastName = "de Tal"
            };
            var employee = new Employee()
            {
                Occupation = "Estagiário",
                AppUser = appUser
            };
            _roleManager.CreateAsync(new IdentityRole("Employee")).Wait();
            _userManager.CreateAsync(appUser, "Teste!23").Wait();
            _userManager.AddToRoleAsync(appUser, "Employee");
            _dbContext.Employee.Add(employee);
            _dbContext.SaveChanges();

            var vm = new RegisterViewModel()
            {
                Email = "estagiario@email.com",
                Occupation = "Estagiário",
                FirstName = "Siclano",
                LastName = "de Tal",
                Password = "Teste!23"
            };

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new EmployeesController(_dbContext, _userManager)
            {
                ObjectValidator = objectValidator.Object
            };
            Helpers.SetupUser(controller, "admin@email.com");

            // Act
            var response = (BadRequestObjectResult)controller.PostEmployee(vm).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            ((SerializableError)response.Value).ContainsKey("Email").Should().BeTrue();
        }

    }
}
