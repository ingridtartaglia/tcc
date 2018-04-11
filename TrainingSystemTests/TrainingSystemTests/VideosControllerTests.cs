using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.IO;
using System.Net;
using System.Threading;
using TrainingSystem.Controllers;
using TrainingSystem.Data;
using TrainingSystem.Models;
using TrainingSystem.ViewModels;
using Xunit;

namespace TrainingSystem.Tests.ControllerTests
{
    public class VideosControllerTests : IDisposable
    {
        private readonly TrainingSystemContext _dbContext;
        private readonly IHostingEnvironment _environment;

        public VideosControllerTests()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var optionBuilder = new DbContextOptionsBuilder<TrainingSystemContext>();
            optionBuilder.EnableSensitiveDataLogging(true);
            optionBuilder.UseSqlite(connection);
            _dbContext = new TrainingSystemContext(optionBuilder.Options);
            _dbContext.Database.EnsureCreated();
            var mockEnvironment = new Mock<IHostingEnvironment>();
            mockEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockEnvironment.Setup(m => m.ContentRootPath).Returns("../../../../TrainingSystem");
            _environment = mockEnvironment.Object;
        }

        [Fact]
        public void PostVideo()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var content = "content";
            var fileName = "teste.mp4";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => ms.CopyToAsync(stream))
                .Verifiable();

            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var lesson = new Lesson() {
                CourseId = 1,
                LessonId = 1,
                Name = "Unidade 1"
            };
            var videoToAdd = new VideoViewModel() {
                LessonId = 1,
                Name = "Introdução à programação",
                File = fileMock.Object
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new VideosController(_dbContext, _environment) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (CreatedAtActionResult)controller.PostVideo(videoToAdd).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.Created, response.StatusCode);
            ((Video)response.Value).VideoId.Should().NotBe(0);
            ((Video)response.Value).Name.Should().Be("Introdução à programação");
        }

        [Fact]
        public void CreateInvalidVideo()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var lesson = new Lesson() {
                CourseId = 1,
                LessonId = 1,
                Name = "Unidade 1"
            };
            var videoToAdd = new VideoViewModel() {
                LessonId = 1,
                Name = "Introdução à programação",
                File = null
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new VideosController(_dbContext, _environment) {
                ObjectValidator = objectValidator.Object
            };
            controller.ModelState.AddModelError("File", "The File field is required");

            // Act
            var response = (BadRequestObjectResult)controller.PostVideo(videoToAdd).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            ((SerializableError)response.Value).ContainsKey("File").Should().BeTrue();
        }

        [Fact]
        public void DeleteVideo()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var lesson = new Lesson() {
                CourseId = 1,
                LessonId = 1,
                Name = "Unidade 1"
            };
            var video = new Video() {
                VideoId = 1,
                LessonId = 1,
                Name = "Introdução à programação",
                FileName = "introduction.mp4"
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Video.Add(video);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new VideosController(_dbContext, _environment) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (OkObjectResult)controller.DeleteVideo(1).Result;

            // Assert
            response.StatusCode.Should().Be(200);
            _dbContext.Video.Find(1).Should().BeNull();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
