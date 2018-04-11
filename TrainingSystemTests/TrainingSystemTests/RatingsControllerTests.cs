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
    public class RatingsControllerTests : IDisposable
    {
        private readonly TrainingSystemContext _dbContext;

        public RatingsControllerTests()
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
        public void GetRatings()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            var rating1 = new Rating() {
                CourseId = 1,
                Grade = 5
            };
            var rating2 = new Rating() {
                CourseId = 1,
                Grade = 4
            };
            _dbContext.Course.Add(course);
            _dbContext.Rating.Add(rating1);
            _dbContext.Rating.Add(rating2);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new RatingsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = controller.GetRatings();

            // Assert
            response.Should().HaveCount(2);
        }

        [Fact]
        public void PostRating()
        {
            // Arrange
            var course = new Course() {
                CourseId = 1,
                Name = "Curso 1",
                Category = "Outros",
                Instructor = "Fulano"
            };
            _dbContext.Course.Add(course);
            _dbContext.SaveChanges();

            var ratingToAdd = new Rating() {
                CourseId = 1,
                Grade = 5
            };

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new RatingsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (CreatedAtActionResult)controller.PostRating(ratingToAdd).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.Created, response.StatusCode);
            ((Rating)response.Value).RatingId.Should().NotBe(0);
            ((Rating)response.Value).Grade.Should().Be(5);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}

