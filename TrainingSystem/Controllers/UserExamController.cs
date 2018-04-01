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

        // GET: api/UserExams
        [HttpGet]
        public IEnumerable<UserExam> GetUserExams()
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.FindByEmailAsync(email).Result;
            var employee = _context.Employee.SingleOrDefault(e => e.AppUserId == user.Id);
            return _context.UserExam
                .Where(ue => ue.EmployeeId == employee.EmployeeId)
                .Include(ue => ue.Exam);
        }

        // GET: api/UserExams/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserExam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userExam = await _context.UserExam
                .SingleOrDefaultAsync(ue => ue.UserExamId == id);

            if (userExam == null)
            {
                return NotFound();
            }

            return Ok(userExam);
        }

        // POST: api/UserExams
        [HttpPost]
        public async Task<IActionResult> PostUserExam([FromBody] UserExam userExam)
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.FindByEmailAsync(email).Result;
            var employee = _context.Employee.SingleOrDefault(e => e.AppUserId == user.Id);
            userExam.EmployeeId = employee.EmployeeId;

            // check if user is approved
            var rightQuestionsCount = 0;
            foreach (var userChoice in userExam.UserExamChoices) {
                // for each answers given by the user, check in the database if it´s the correct one
                var choice = _context.QuestionChoice
                        .SingleOrDefault(qc => qc.QuestionChoiceId == userChoice.QuestionChoiceId);
                
                // if it´s correct add to count
                if(choice.IsCorrect) {
                    rightQuestionsCount++;
                }
            }

            // if correct answers are more than 70% user is approved
            userExam.IsApproved = (rightQuestionsCount / userExam.UserExamChoices.Count) >= 0.7d;

            _context.UserExam.Add(userExam);
            await _context.SaveChangesAsync();

            return Ok(userExam);
        }
    }
}