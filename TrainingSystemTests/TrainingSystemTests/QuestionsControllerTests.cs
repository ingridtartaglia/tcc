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
    public class QuestionsControllerTests : IDisposable
    {
        private readonly TrainingSystemContext _dbContext;

        public QuestionsControllerTests()
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
        public void PostQuestion()
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
            var exam = new Exam() {
                LessonId = 1,
                ExamId = 1
            };
            var questionToAdd = new Question() {
                ExamId = 1,
                Name = "Quem foi o primeiro programador?"
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Exam.Add(exam);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new QuestionsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (CreatedAtActionResult)controller.PostQuestion(questionToAdd).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.Created, response.StatusCode);
            ((Question)response.Value).QuestionId.Should().NotBe(0);
            ((Question)response.Value).Name.Should().Be("Quem foi o primeiro programador?");
        }

        [Fact]
        public void CreateInvalidQuestion()
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
            var exam = new Exam() {
                LessonId = 1,
                ExamId = 1
            };
            var questionToAdd = new Question() {
                ExamId = 1,
                Name = ""
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Exam.Add(exam);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new QuestionsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };
            controller.ModelState.AddModelError("Name", "Nome obrigatório.");

            // Act
            var response = (BadRequestObjectResult)controller.PostQuestion(questionToAdd).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            ((SerializableError)response.Value).ContainsKey("Name").Should().BeTrue();
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
            var lesson = new Lesson() {
                CourseId = 1,
                LessonId = 1,
                Name = "Unidade 1"
            };
            var exam = new Exam() {
                LessonId = 1,
                ExamId = 1
            };
            var question = new Question() {
                ExamId = 1,
                QuestionId = 1,
                Name = "Quem foi o primeiro programador?"
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Exam.Add(exam);
            _dbContext.Question.Add(question);
            _dbContext.SaveChanges();


            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new QuestionsController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (OkObjectResult)controller.DeleteQuestion(1).Result;

            // Assert
            response.StatusCode.Should().Be(200);
            _dbContext.Question.Find(1).Should().BeNull();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
