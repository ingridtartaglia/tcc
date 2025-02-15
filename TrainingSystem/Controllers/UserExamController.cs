using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingSystem.Data;
using TrainingSystem.Models;

namespace TrainingSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/UserExams")]
    public class UserExamController : Controller
    {
        private readonly TrainingSystemContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserExamController(TrainingSystemContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/UserExams/5
        [HttpGet("{id}")]
        public bool IsApproved([FromRoute] int id)
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.FindByEmailAsync(email).Result;
            var employee = _context.Employee.SingleOrDefault(e => e.AppUserId == user.Id);
            var userExam = _context.UserExam
                .SingleOrDefault(ue => ue.EmployeeId == employee.EmployeeId && ue.ExamId == id && ue.IsApproved);

            if (userExam == null) return false;

            return userExam.IsApproved;
        }

        // POST: api/UserExams
        [HttpPost]
        public async Task<IActionResult> PostUserExam([FromBody] UserExam userExam)
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.FindByEmailAsync(email).Result;
            var employee = _context.Employee.SingleOrDefault(e => e.AppUserId == user.Id);

            // checking if this user is already approved for this exam
            var lastExam = _context.UserExam
                                   .Where(ue => ue.ExamId == userExam.ExamId)
                                   .OrderByDescending(e => e.SubmissionDate)
                                   .FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
            
            if (lastExam != null && lastExam.IsApproved) {
                return BadRequest("Você já foi aprovado nessa atividade.");
            }

            userExam.EmployeeId = employee.EmployeeId;
            userExam.SubmissionDate = DateTime.Now;

            // check if user is approved
            var rightQuestionsCount = 0;
            foreach (var userChoice in userExam.UserExamChoices) {
                // for each answers given by the user, check in the database if it's the correct one
                var choice = _context.QuestionChoice
                        .SingleOrDefault(qc => qc.QuestionChoiceId == userChoice.QuestionChoiceId);
                
                // if it's correct add to count
                if(choice.IsCorrect) {
                    rightQuestionsCount++;
                }
            }

            // if correct answers are more than 70% user is approved
            userExam.IsApproved = Decimal.Divide(rightQuestionsCount, userExam.UserExamChoices.Count) >= 0.7m;

            _context.UserExam.Add(userExam);
            await _context.SaveChangesAsync();

            return Ok(userExam);
        }
    }
}