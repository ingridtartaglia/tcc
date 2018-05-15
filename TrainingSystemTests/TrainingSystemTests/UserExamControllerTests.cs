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
using System.Collections.Generic;
using System.Net;
using TrainingSystem.Controllers;
using TrainingSystem.Data;
using TrainingSystem.Models;
using Xunit;

namespace TrainingSystemTests.TrainingSystemTests
{
    public class UserExamControllerTests
    {
        private readonly TrainingSystemContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        public UserExamControllerTests()
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
        public void PostUserExam()
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
            _userManager.CreateAsync(appUser, "Teste!23").Wait();
            _dbContext.Employee.Add(employee);
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Exam.Add(exam);
            _dbContext.Question.Add(question);
            _dbContext.QuestionChoice.Add(questionChoice);
            _dbContext.UserExamChoice.Add(userExamChoice);
            _dbContext.SaveChanges();

            var userExamToCreate = new UserExam(){
                EmployeeId = 1,
                ExamId = 1,
                UserExamChoices = new List<UserExamChoice>{userExamChoice},
                IsApproved = true,
                SubmissionDate = new DateTime(2018, 05, 14, 12, 00, 00)
            };

            var controller = new UserExamController(_dbContext, _userManager);
            Helpers.SetupUser(controller, "employee@email.com");

            // Act
            var response = (OkObjectResult)controller.PostUserExam(userExamToCreate).Result;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void PostInvalidUserExam()
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
            _userManager.CreateAsync(appUser, "Teste!23").Wait();
            _dbContext.Employee.Add(employee);
            _dbContext.Course.Add(course);
            _dbContext.Lesson.Add(lesson);
            _dbContext.Exam.Add(exam);
            _dbContext.Question.Add(question);
            _dbContext.QuestionChoice.Add(questionChoice);
            _dbContext.UserExamChoice.Add(userExamChoice);
            _dbContext.UserExam.Add(userExam);
            _dbContext.SaveChanges();

            var userExamToCreate = new UserExam()
            {
                EmployeeId = 1,
                ExamId = 1,
                UserExamChoices = new List<UserExamChoice> { userExamChoice },
                IsApproved = true,
                SubmissionDate = new DateTime(2018, 05, 14, 12, 00, 00)
            };

            var controller = new UserExamController(_dbContext, _userManager);
            Helpers.SetupUser(controller, "employee@email.com");

            // Act
            var response = (BadRequestObjectResult)controller.PostUserExam(userExamToCreate).Result;

            // Assert
            response.StatusCode.Should().Be(400);
        }
    }
}
