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
    public class CourseSubscriptionControllerTests
    {
        private readonly TrainingSystemContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public CourseSubscriptionControllerTests()
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
        }

        [Fact]
        public void GetUserSubscriptions()
        {
            // Arrange
            var appUser = new AppUser()
            {
                Id = "1",
                Email = "employee@email.com",
                UserName = "employee@email.com"
            };
            var course1 = new Course()
            {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var course2 = new Course()
            {
                CourseId = 2,
                Name = "Curso 2",
                Category = "Asp.net",
                Instructor = "Fulano"
            };
            var employee = new Employee()
            {
                EmployeeId = 1,
                AppUserId = "1",
                Occupation = "Estagiário"
            };
            var courseSubscription1 = new CourseSubscription()
            {
                EmployeeId = 1,
                CourseId = 1
            };
            var courseSubscription2 = new CourseSubscription()
            {
                EmployeeId = 1,
                CourseId = 2
            };
            _userManager.CreateAsync(appUser, "Teste!23").Wait();
            _dbContext.Course.Add(course1);
            _dbContext.Course.Add(course2);
            _dbContext.Employee.Add(employee);
            _dbContext.CourseSubscription.Add(courseSubscription1);
            _dbContext.CourseSubscription.Add(courseSubscription2);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));

            var controller = new CourseSubscriptionController(_dbContext, _userManager)
            {
                ObjectValidator = objectValidator.Object
            };
            Helpers.SetupUser(controller, "employee@email.com");

            // Act
            var response = controller.GetUserSubscriptions();

            // Assert
            response.Should().HaveCount(2);
        }

        [Fact]
        public void PostSubscription()
        {
            // Arrange
            var appUser = new AppUser()
            {
                Id = "1",
                Email = "employee@email.com",
                UserName = "employee@email.com"
            };
            var employee = new Employee()
            {
                EmployeeId = 1,
                AppUserId = "1",
                Occupation = "Estagiário"
            };
            var course = new Course()
            {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            _userManager.CreateAsync(appUser, "Teste!23").Wait();
            _dbContext.Employee.Add(employee);
            _dbContext.Course.Add(course);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new CourseSubscriptionController(_dbContext, _userManager)
            {
                ObjectValidator = objectValidator.Object
            };
            Helpers.SetupUser(controller, "employee@email.com");

            // Act
            var response = (OkResult)controller.PostSubscription(course.CourseId).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        }
    }
}
