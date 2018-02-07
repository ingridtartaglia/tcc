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
    public class QuestionChoicesControllerTests : IDisposable
    {
        private readonly TrainingSystemContext _dbContext;

        public QuestionChoicesControllerTests()
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
        public void GetQuestionChoices()
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
                QuestionName = "Quem foi o primeiro programador?"
            };
            var questionChoice1 = new QuestionChoice() {
                QuestionId = 1,
                QuestionChoiceName = "Ada Lovelace",
                IsCorrect = true
            };
            var questionChoice2 = new QuestionChoice() {
                QuestionId = 1,
                QuestionChoiceName = "Bill Gates",
                IsCorrect = false
            };
            var questionChoice3 = new QuestionChoice() {
                QuestionId = 1,
                QuestionChoiceName = "Steve Jobs",
                IsCorrect = false
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Exam.Add(exam);
            _dbContext.Question.Add(question);
            _dbContext.QuestionChoice.Add(questionChoice1);
            _dbContext.QuestionChoice.Add(questionChoice2);
            _dbContext.QuestionChoice.Add(questionChoice3);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new QuestionChoicesController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = controller.GetQuestionChoices();

            // Assert
            response.Should().HaveCount(3);
        }

        [Fact]
        public void GetQuestionChoice()
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
                QuestionName = "Quem foi o primeiro programador?"
            };
            var questionChoice1 = new QuestionChoice() {
                QuestionId = 1,
                QuestionChoiceName = "Ada Lovelace",
                IsCorrect = true
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Exam.Add(exam);
            _dbContext.Question.Add(question);
            _dbContext.QuestionChoice.Add(questionChoice1);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new QuestionChoicesController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (OkObjectResult)controller.GetQuestionChoice(1).Result;

            // Assert
            ((QuestionChoice)response.Value).QuestionChoiceId.Should().Be(1);
        }

        [Fact]
        public void GetNotFoundQuestionChoice()
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
                QuestionName = "Quem foi o primeiro programador?"
            };
            var questionChoice1 = new QuestionChoice() {
                QuestionId = 1,
                QuestionChoiceName = "Ada Lovelace",
                IsCorrect = true
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Exam.Add(exam);
            _dbContext.Question.Add(question);
            _dbContext.QuestionChoice.Add(questionChoice1);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new QuestionChoicesController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (NotFoundResult)controller.GetQuestionChoice(50).Result;

            // Assert
            response.StatusCode.Should().Be(404);
        }

        [Fact]
        public void PostQuestionChoice()
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
                QuestionName = "Quem foi o primeiro programador?"
            };
            var questionChoiceToAdd = new QuestionChoice() {
                QuestionId = 1,
                QuestionChoiceName = "Grace Hopper",
                IsCorrect = false
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
            var controller = new QuestionChoicesController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (CreatedAtActionResult)controller.PostQuestionChoice(questionChoiceToAdd).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.Created, response.StatusCode);
            ((QuestionChoice)response.Value).QuestionChoiceId.Should().NotBe(0);
            ((QuestionChoice)response.Value).QuestionChoiceName.Should().Be("Grace Hopper");
        }

        [Fact]
        public void CreateInvalidQuestionChoice()
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
                QuestionName = "Quem foi o primeiro programador?"
            };
            var questionChoiceToAdd = new QuestionChoice() {
                QuestionId = 1,
                QuestionChoiceName = "",
                IsCorrect = true
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
            var controller = new QuestionChoicesController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };
            controller.ModelState.AddModelError("QuestionChoiceName", "The QuestionChoiceName field is required");

            // Act
            var response = (BadRequestObjectResult)controller.PostQuestionChoice(questionChoiceToAdd).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            ((SerializableError)response.Value).ContainsKey("QuestionChoiceName").Should().BeTrue();
        }

        [Fact]
        public void PutQuestionChoice()
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
                QuestionName = "Quem foi o primeiro programador?"
            };
            var questionChoice = new QuestionChoice() {
                QuestionId = 1,
                QuestionChoiceId = 1,
                QuestionChoiceName = "Ada Lovelace",
                IsCorrect = true
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Exam.Add(exam);
            _dbContext.Question.Add(question);
            _dbContext.QuestionChoice.Add(questionChoice);
            _dbContext.SaveChanges();

            var questionChoiceToUpdate = _dbContext.QuestionChoice.FirstOrDefaultAsync(q => q.QuestionChoiceId == 1).Result;
            questionChoiceToUpdate.QuestionChoiceName = "Ada";

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new QuestionChoicesController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (NoContentResult)controller.PutQuestionChoice(1, questionChoiceToUpdate).Result;

            // Assert
            response.StatusCode.Should().Be(204);
        }

        [Fact]
        public void UpdateNotFoundQuestionChoice()
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
                QuestionName = "Quem foi o primeiro programador?"
            };
            var questionChoice = new QuestionChoice() {
                QuestionId = 1,
                QuestionChoiceId = 1,
                QuestionChoiceName = "Ada Lovelace",
                IsCorrect = true
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Exam.Add(exam);
            _dbContext.Question.Add(question);
            _dbContext.QuestionChoice.Add(questionChoice);
            _dbContext.SaveChanges();

            var questionChoiceToUpdate = new QuestionChoice() {
                QuestionId = 1,
                QuestionChoiceId = 50,
                QuestionChoiceName = "Ada Lovelace",
                IsCorrect = true
            };
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new QuestionChoicesController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (NotFoundResult)controller.PutQuestionChoice(50, questionChoiceToUpdate).Result;

            // Assert
            response.StatusCode.Should().Be(404);
        }

        [Fact]
        public void UpdateBadRequestQuestionChoice()
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
                QuestionName = "Quem foi o primeiro programador?"
            };
            var questionChoice = new QuestionChoice() {
                QuestionId = 1,
                QuestionChoiceId = 1,
                QuestionChoiceName = "Ada Lovelace",
                IsCorrect = true
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Exam.Add(exam);
            _dbContext.Question.Add(question);
            _dbContext.QuestionChoice.Add(questionChoice);
            _dbContext.SaveChanges();

            var questionChoiceToUpdate = new QuestionChoice() {
                QuestionId = 1,
                QuestionChoiceId = 1,
                QuestionChoiceName = "Ada Lovelace",
                IsCorrect = true
            };
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new QuestionChoicesController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (BadRequestResult)controller.PutQuestionChoice(50, questionChoiceToUpdate).Result;

            // Assert
            response.StatusCode.Should().Be(400);
        }

        [Fact]
        public void DeleteQuestionChoice()
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
                QuestionName = "Quem foi o primeiro programador?"
            };
            var questionChoice = new QuestionChoice() {
                QuestionId = 1,
                QuestionChoiceId = 1,
                QuestionChoiceName = "Ada Lovelace",
                IsCorrect = true
            };
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Exam.Add(exam);
            _dbContext.Question.Add(question);
            _dbContext.QuestionChoice.Add(questionChoice);
            _dbContext.SaveChanges();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            var controller = new QuestionChoicesController(_dbContext) {
                ObjectValidator = objectValidator.Object
            };

            // Act
            var response = (OkObjectResult)controller.DeleteQuestionChoice(1).Result;

            // Assert
            response.StatusCode.Should().Be(200);
            _dbContext.QuestionChoice.Find(1).Should().BeNull();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
