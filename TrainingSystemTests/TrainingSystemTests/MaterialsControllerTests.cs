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
    public class MaterialsControllerTests : IDisposable
    {
        private readonly TrainingSystemContext _dbContext;

        public MaterialsControllerTests()
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
        public void GetMaterials()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var material1 = new Material() {
                CourseId = 1,
                Name = "Introdução à programação",
                FileName = "introduction.pdf"
            };
            var material2 = new Material() {
                CourseId = 1,
                Name = "Escrevendo o seu primeiro hello world",
                FileName = "helloworld.pdf"
            };
            _dbContext.Course.Add(course);
            _dbContext.Material.Add(material1);
            _dbContext.Material.Add(material2);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new MaterialsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = controller.GetMaterials();

            // Assert
            response.Should().HaveCount(2);
        }

        [Fact]
        public void GetMaterial()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var material = new Material() {
                CourseId = 1,
                Name = "Introdução à programação",
                FileName = "introduction.pdf"
            };
            _dbContext.Course.Add(course);
            _dbContext.Material.Add(material);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new MaterialsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (OkObjectResult)controller.GetMaterial(1).Result;

            // Assert
            ((Material)response.Value).MaterialId.Should().Be(1);
        }

        [Fact]
        public void GetNotFoundMaterial()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var material = new Material() {
                CourseId = 1,
                Name = "Introdução à programação",
                FileName = "introduction.mp4"
            };
            _dbContext.Course.Add(course);
            _dbContext.Material.Add(material);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new MaterialsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (NotFoundResult)controller.GetMaterial(50).Result;

            // Assert
            response.StatusCode.Should().Be(404);
        }

        [Fact]
        public void PostMaterial()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var materialToAdd = new Material() {
                CourseId = 1,
                Name = "Introdução à programação",
                FileName = "introduction.pdf"
            };
            _dbContext.Course.Add(course);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new MaterialsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (CreatedAtActionResult)controller.PostMaterial(materialToAdd).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.Created, response.StatusCode);
            ((Material)response.Value).MaterialId.Should().NotBe(0);
            ((Material)response.Value).Name.Should().Be("Introdução à programação");
        }

        [Fact]
        public void CreateInvalidMaterial()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var materialToAdd = new Material() {
                CourseId = 1,
                Name = "Introdução à programação",
                FileName = ""
            };
            _dbContext.Course.Add(course);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new MaterialsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };
            controller.ModelState.AddModelError("FileName", "The FileName field is required");

            // Act
            var response = (BadRequestObjectResult)controller.PostMaterial(materialToAdd).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            ((SerializableError)response.Value).ContainsKey("FileName").Should().BeTrue();
        }

        [Fact]
        public void PutMaterial()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var material = new Material() {
                CourseId = 1,
                MaterialId = 1,
                Name = "Introdução à programação",
                FileName = "introduction.pdf"
            };
            _dbContext.Course.Add(course);
            _dbContext.Material.Add(material);
            _dbContext.SaveChanges();

            var materialToUpdate = _dbContext.Material.FirstOrDefaultAsync(m => m.MaterialId == 1).Result;
            materialToUpdate.Name = "Introdução";

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new MaterialsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (NoContentResult)controller.PutMaterial(1, materialToUpdate).Result;

            // Assert
            response.StatusCode.Should().Be(204);
        }

        [Fact]
        public void UpdateNotFoundMaterial()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var material = new Material() {
                CourseId = 1,
                MaterialId = 1,
                Name = "Introdução à programação",
                FileName = "introduction.pdf"
            };
            _dbContext.Course.Add(course);
            _dbContext.Material.Add(material);
            _dbContext.SaveChanges();

            var materialToUpdate = new Material() {
                CourseId = 1,
                MaterialId = 50,
                Name = "Introdução",
                FileName = "introduction.pdf"
            };
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new MaterialsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (NotFoundResult)controller.PutMaterial(50, materialToUpdate).Result;

            // Assert
            response.StatusCode.Should().Be(404);
        }

        [Fact]
        public void UpdateBadRequestMaterial()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var material = new Material() {
                CourseId = 1,
                MaterialId = 1,
                Name = "Introdução à programação",
                FileName = "introduction.pdf"
            };
            _dbContext.Course.Add(course);
            _dbContext.Material.Add(material);
            _dbContext.SaveChanges();

            var materialToUpdate = new Material() {
                CourseId = 1,
                MaterialId = 1,
                Name = "Introdução",
                FileName = "introduction.pdf"
            };
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new MaterialsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (BadRequestResult)controller.PutMaterial(50, materialToUpdate).Result;

            // Assert
            response.StatusCode.Should().Be(400);
        }

        [Fact]
        public void DeleteMaterial()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var material = new Material() {
                CourseId = 1,
                Name = "Introdução à programação",
                FileName = "introduction.mp4"
            };
            _dbContext.Course.Add(course);
            _dbContext.Material.Add(material);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new MaterialsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (OkObjectResult)controller.DeleteMaterial(1).Result;

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

