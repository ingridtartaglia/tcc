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
    [Route("api/Subscription")]
    public class CourseSubscriptionController : Controller
    {
        private readonly TrainingSystemContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CourseSubscriptionController(TrainingSystemContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/UserSubscriptions
        [HttpGet("UserSubscriptions")]
        public IEnumerable<CourseSubscription> GetUserSubscriptions()
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.FindByEmailAsync(email).Result;
            var employee = _context.Employee.SingleOrDefault(e => e.AppUserId == user.Id);
            return _context.CourseSubscription
                .Where(cs => cs.EmployeeId == employee.EmployeeId)
                .Include(c => c.Course);
        }

        // POST: api/CourseSubscriptions/5
        [HttpPost("CourseSubscriptions")]
        public async Task<IActionResult> PostSubscription([FromBody] int id)
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.FindByEmailAsync(email).Result;
            var employee = _context.Employee.SingleOrDefault(e => e.AppUserId == user.Id);
            var courseSubscription = new CourseSubscription();
            courseSubscription.CourseId = id;
            courseSubscription.EmployeeId = employee.EmployeeId;

            _context.CourseSubscription.Add(courseSubscription);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}