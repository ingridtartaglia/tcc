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
using Xunit;

namespace TrainingSystemTests.TrainingSystemTests
{
    public class VideoWatchControllerTests
    {
        private readonly TrainingSystemContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public VideoWatchControllerTests()
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
        public void PostVideoWatch()
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
            _userManager.CreateAsync(appUser, "Teste!23").Wait();
            _dbContext.Employee.Add(employee);
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Video.Add(video);
            _dbContext.SaveChanges();

            var controller = new VideoWatchController(_dbContext, _userManager);
            Helpers.SetupUser(controller, "employee@email.com");

            // Act
            var response = (OkObjectResult)controller.PostUserVideoWatch(video.VideoId).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void PutUserVideoWatch()
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
            _userManager.CreateAsync(appUser, "Teste!23").Wait();
            _dbContext.Employee.Add(employee);
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Video.Add(video);
            _dbContext.VideoWatch.Add(videoWatch);
            _dbContext.SaveChanges();

            var controller = new VideoWatchController(_dbContext, _userManager);
            Helpers.SetupUser(controller, "employee@email.com");

            // Act
            var videoWatchToUpdate = _dbContext.VideoWatch.SingleOrDefaultAsync(vw => vw.EmployeeId == 1 && vw.VideoId == 1).Result;
            videoWatchToUpdate.IsCompleted = false;
            var response = (NoContentResult)controller.PutUserVideoWatch(videoWatchToUpdate).Result;

            // Assert
            response.StatusCode.Should().Be(204);
        }

        [Fact]
        public void UpdateNotFoundVideoWatch()
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
            _userManager.CreateAsync(appUser, "Teste!23").Wait();
            _dbContext.Employee.Add(employee);
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Video.Add(video);
            _dbContext.SaveChanges();

            var controller = new VideoWatchController(_dbContext, _userManager);
            Helpers.SetupUser(controller, "employee@email.com");

            // Act
            var videoWatchToUpdate = new VideoWatch()
            {
                EmployeeId = 1,
                IsCompleted = false,
                VideoId = 1
            };
            var response = (NotFoundResult)controller.PutUserVideoWatch(videoWatchToUpdate).Result;

            // Assert
            response.StatusCode.Should().Be(404);
        }
    }
}
