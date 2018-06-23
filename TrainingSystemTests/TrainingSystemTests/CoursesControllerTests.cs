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
using TrainingSystem.ViewModels;
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
        public void GetCourseForAdmin()
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
        public void GetCourseForEmployee()
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
            var courseSubscription = new CourseSubscription()
            {
                EmployeeId = 1,
                CourseId = 1
            };
            var lesson = new Lesson()
            {
                CourseId = 1,
                LessonId = 1,
                Name = "Unidade 1"
            };
            var video = new Video()
            {
                VideoId = 1,
                LessonId = 1,
                Name = "Introdução à programação",
                FileName = "introduction.mp4"
            };
            var videoWatch = new VideoWatch()
            {
                EmployeeId = 1,
                IsCompleted = false,
                VideoId = 1
            };
            var exam = new Exam()
            {
                LessonId = 1,
                ExamId = 1
            };
            var question = new Question()
            {
                ExamId = 1,
                QuestionId = 1,
                Name = "Quem foi o primeiro programador?"
            };
            var questionChoice = new QuestionChoice()
            {
                QuestionChoiceId = 1,
                QuestionId = 1,
                Name = "Steve Jobs",
                IsCorrect = false
            };
            var userExamChoice = new UserExamChoice()
            {
                UserExamChoiceId = 1,
                QuestionChoiceId = 1,
                UserExamId = 1
            };
            var userExam = new UserExam()
            {
                EmployeeId = 1,
                ExamId = 1,
                UserExamChoices = new List<UserExamChoice> { userExamChoice },
                IsApproved = true,
                SubmissionDate = new DateTime(2018, 05, 14, 12, 00, 00)
            };
            _roleManager.CreateAsync(new IdentityRole("Employee")).Wait();
            _userManager.CreateAsync(appUser, "Teste!23").Wait();
            _userManager.AddToRoleAsync(appUser, "Employee").Wait();
            _dbContext.Employee.Add(employee);
            _dbContext.Course.Add(course);
            _dbContext.CourseSubscription.Add(courseSubscription);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Video.Add(video);
            _dbContext.VideoWatch.Add(videoWatch);
            _dbContext.Exam.Add(exam);
            _dbContext.Question.Add(question);
            _dbContext.QuestionChoice.Add(questionChoice);
            _dbContext.UserExamChoice.Add(userExamChoice);
            _dbContext.UserExam.Add(userExam);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new CoursesController(_dbContext, _userManager)
            {
                ObjectValidator = objectValidator.Object
            };
            Helpers.SetupUser(controller, "employee@email.com");

            // Act
            var response = (OkObjectResult)controller.GetCourse(1).Result;

            // Assert
            ((CourseViewModel)response.Value).CourseId.Should().Be(1);
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
            controller.ModelState.AddModelError("Name", "Nome obrigatório.");

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
