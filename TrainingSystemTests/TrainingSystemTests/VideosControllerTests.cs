using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Net;
using TrainingSystem.Controllers;
using TrainingSystem.Data;
using TrainingSystem.Models;
using Xunit;

namespace TrainingSystem.Tests.ControllerTests
{
    public class VideosControllerTests : IDisposable
    {
        private readonly TrainingSystemContext _dbContext;

        public VideosControllerTests()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var optionBuilder = new DbContextOptionsBuilder<TrainingSystemContext>();
            optionBuilder.EnableSensitiveDataLogging(true);
            optionBuilder.UseSqlite(connection);
            _dbContext = new TrainingSystemContext(optionBuilder.Options);
            _dbContext.Database.EnsureCreated();
        }

        [Fact]
        public void GetVideos()
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
            var video1 = new Video() {
                LessonId = 1,
                Name = "Introdução à programação",
                FileName = "introduction.mp4"
            };
            var video2 = new Video() {
                LessonId = 1,
                Name = "Escrevendo o seu primeiro hello world",
                FileName = "helloworld.mp4"
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Video.Add(video1);
            _dbContext.Video.Add(video2);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new VideosController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = controller.GetVideos();

            // Assert
            response.Should().HaveCount(2);
        }

        [Fact]
        public void GetVideo()
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
            var controller = new VideosController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (OkObjectResult)controller.GetVideo(1).Result;

            // Assert
            ((Video)response.Value).VideoId.Should().Be(1);
        }

        [Fact]
        public void GetNotFoundVideo()
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
            var controller = new VideosController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (NotFoundResult)controller.GetVideo(50).Result;

            // Assert
            response.StatusCode.Should().Be(404);
        }

        [Fact]
        public void PostVideo()
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
            var videoToAdd = new Video() {
                LessonId = 1,
                Name = "Introdução à programação",
                FileName = "introduction.mp4"
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new VideosController(_dbContext) {
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
            var videoToAdd = new Video() {
                LessonId = 1,
                Name = "Introdução à programação",
                FileName = ""
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new VideosController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };
            controller.ModelState.AddModelError("FileName", "The FileName field is required");

            // Act
            var response = (BadRequestObjectResult)controller.PostVideo(videoToAdd).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            ((SerializableError)response.Value).ContainsKey("FileName").Should().BeTrue();
        }

        [Fact]
        public void PutVideo()
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

            var videoToUpdate = _dbContext.Video.FirstOrDefaultAsync(v => v.VideoId == 1).Result;
            videoToUpdate.Name = "Introdução";

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new VideosController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (NoContentResult)controller.PutVideo(1, videoToUpdate).Result;

            // Assert
            response.StatusCode.Should().Be(204);
        }

        [Fact]
        public void UpdateNotFoundVideo()
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

            var videoToUpdate = new Video() {
                LessonId = 1,
                VideoId = 50,
                Name = "Introdução",
                FileName = "introduction.mp4"
            };
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new VideosController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (NotFoundResult)controller.PutVideo(50, videoToUpdate).Result;

            // Assert
            response.StatusCode.Should().Be(404);
        }

        [Fact]
        public void UpdateBadRequestVideo()
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

            var videoToUpdate = new Video() {
                LessonId = 1,
                VideoId = 1,
                Name = "Introdução",
                FileName = "introduction.mp4"
            };
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new VideosController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (BadRequestResult)controller.PutVideo(50, videoToUpdate).Result;

            // Assert
            response.StatusCode.Should().Be(400);
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
            var controller = new VideosController(_dbContext) {
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
