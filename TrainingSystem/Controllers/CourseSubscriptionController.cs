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

        // GET: api/CourseSubscriptions
        [HttpGet("CourseSubscriptions")]
        public IEnumerable<CourseSubscription> GetCourseSubscriptions(int id)
        {
            return _context.CourseSubscription
                .Where(cs => cs.CourseId == id)
                .Include(e => e.Employee);
        }

        // POST: api/CourseSubscriptions/5
        [HttpPost]
        public async Task<IActionResult> PostSubscription([FromBody] CourseSubscription courseSubscription)
        {
            _context.CourseSubscription.Add(courseSubscription);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/CourseSubscriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription([FromRoute] int id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var courseSubscription = await _context.CourseSubscription
                .SingleOrDefaultAsync(cs => cs.Employee.AppUserId == user.Id && cs.CourseId == id);
            if (courseSubscription == null)
            {
                return NotFound();
            }

            _context.CourseSubscription.Remove(courseSubscription);
            await _context.SaveChangesAsync();

            return Ok(courseSubscription);
        }
    }
}