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

namespace TrainingSystemTests.TrainingSystemTests
{
    public class KeywordsControllerTests : IDisposable
    {
        private readonly TrainingSystemContext _dbContext;

        public KeywordsControllerTests()
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
        public void GetKeywords()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var keyword1 = new Keyword() {
                CourseId = 1,
                Name = "Programação"
            };
            var keyword2 = new Keyword() {
                CourseId = 1,
                Name = "Básico"
            };
            _dbContext.Course.Add(course);
            _dbContext.Keyword.Add(keyword1);
            _dbContext.Keyword.Add(keyword2);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new KeywordsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = controller.GetKeywords();

            // Assert
            response.Should().HaveCount(2);
        }

        [Fact]
        public void GetKeyword()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var keyword = new Keyword() {
                CourseId = 1,
                Name = "Programação"
            };
            _dbContext.Course.Add(course);
            _dbContext.Keyword.Add(keyword);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new KeywordsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (OkObjectResult)controller.GetKeyword(1).Result;

            // Assert
            ((Keyword)response.Value).KeywordId.Should().Be(1);
        }

        [Fact]
        public void GetNotFoundKeyword()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var keyword = new Keyword() {
                CourseId = 1,
                Name = "Programação"
            };
            _dbContext.Course.Add(course);
            _dbContext.Keyword.Add(keyword);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new KeywordsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (NotFoundResult)controller.GetKeyword(50).Result;

            // Assert
            response.StatusCode.Should().Be(404);
        }

        [Fact]
        public void PostKeyword()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var keywordToAdd = new Keyword() {
                CourseId = 1,
                Name = "Programação"
            };
            _dbContext.Course.Add(course);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new KeywordsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (CreatedAtActionResult)controller.PostKeyword(keywordToAdd).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.Created, response.StatusCode);
            ((Keyword)response.Value).KeywordId.Should().NotBe(0);
            ((Keyword)response.Value).Name.Should().Be("Programação");
        }

        [Fact]
        public void CreateInvalidKeyword()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var keywordToAdd = new Keyword() {
                CourseId = 1,
                Name = ""
            };
            _dbContext.Course.Add(course);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new KeywordsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };
            controller.ModelState.AddModelError("Name", "The Name field is required");

            // Act
            var response = (BadRequestObjectResult)controller.PostKeyword(keywordToAdd).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            ((SerializableError)response.Value).ContainsKey("Name").Should().BeTrue();
        }

        [Fact]
        public void PutLesson()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var keyword = new Keyword() {
                CourseId = 1,
                KeywordId = 1,
                Name = "Programação"
            };
            _dbContext.Course.Add(course);
            _dbContext.Keyword.Add(keyword);
            _dbContext.SaveChanges();

            var keywordToUpdate = _dbContext.Keyword.FirstOrDefaultAsync(k => k.KeywordId == 1).Result;
            keywordToUpdate.Name = "Básico";

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new KeywordsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (NoContentResult)controller.PutKeyword(1, keywordToUpdate).Result;

            // Assert
            response.StatusCode.Should().Be(204);
        }

        [Fact]
        public void UpdateNotFoundKeyword()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var keyword = new Keyword() {
                CourseId = 1,
                Name = "Programação"
            };
            _dbContext.Course.Add(course);
            _dbContext.Keyword.Add(keyword);
            _dbContext.SaveChanges();

            var keywordToUpdate = new Keyword() {
                CourseId = 1,
                KeywordId = 50,
                Name = "Básico"
            };
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new KeywordsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (NotFoundResult)controller.PutKeyword(50, keywordToUpdate).Result;

            // Assert
            response.StatusCode.Should().Be(404);
        }

        [Fact]
        public void UpdateBadRequestKeyword()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var keyword = new Keyword() {
                CourseId = 1,
                Name = "Programação"
            };
            _dbContext.Course.Add(course);
            _dbContext.Keyword.Add(keyword);
            _dbContext.SaveChanges();

            var keywordToUpdate = new Keyword() {
                CourseId = 1,
                KeywordId = 1,
                Name = "Básico"
            };
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new KeywordsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (BadRequestResult)controller.PutKeyword(50, keywordToUpdate).Result;

            // Assert
            response.StatusCode.Should().Be(400);
        }

        [Fact]
        public void DeleteLesson()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var keyword = new Keyword() {
                CourseId = 1,
                Name = "Programação"
            };
            _dbContext.Course.Add(course);
            _dbContext.Keyword.Add(keyword);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new KeywordsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (OkObjectResult)controller.DeleteKeyword(1).Result;

            // Assert
            response.StatusCode.Should().Be(200);
            _dbContext.Keyword.Find(1).Should().BeNull();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
