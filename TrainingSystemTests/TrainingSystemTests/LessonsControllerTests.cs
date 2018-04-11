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
    public class LessonsControllerTests : IDisposable
    {
        private readonly TrainingSystemContext _dbContext;

        public LessonsControllerTests()
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
        public void GetLesson()
        {
            // Arrange
            var course1 = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            _dbContext.Course.Add(course1);

            var lesson1 = new Lesson() {
                CourseId = 1,
                Name = "Unidade 1"
            };
            _dbContext.Lesson.Add(lesson1);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new LessonsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (OkObjectResult)controller.GetLesson(1).Result;

            // Assert
            ((Lesson)response.Value).CourseId.Should().Be(1);
        }

        [Fact]
        public void GetNotFoundLesson()
        {
            // Arrange
            var course1 = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            _dbContext.Course.Add(course1);

            var lesson1 = new Lesson() {
                CourseId = 1,
                Name = "Unidade 1"
            };
            _dbContext.Lesson.Add(lesson1);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new LessonsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (NotFoundResult)controller.GetLesson(50).Result;

            // Assert
            response.StatusCode.Should().Be(404);
        }

        [Fact]
        public void PostLesson()
        {
            // Arrange
            var course1 = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            _dbContext.Course.Add(course1);

            var lessonToAdd = new Lesson() {
                CourseId = 1,
                Name = "Unidade 1"
            };

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new LessonsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (CreatedAtActionResult)controller.PostLesson(lessonToAdd).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.Created, response.StatusCode);
            ((Lesson)response.Value).LessonId.Should().NotBe(0);
            ((Lesson)response.Value).Name.Should().Be("Unidade 1");
        }

        [Fact]
        public void DeleteLesson()
        {
            // Arrange
            var course1 = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            _dbContext.Course.Add(course1);

            var lesson1 = new Lesson() {
                CourseId = 1,
                Name = "Unidade 1"
            };
            _dbContext.Lesson.Add(lesson1);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new LessonsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (OkObjectResult)controller.DeleteLesson(1).Result;

            // Assert
            response.StatusCode.Should().Be(200);
            _dbContext.Lesson.Find(1).Should().BeNull();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}

