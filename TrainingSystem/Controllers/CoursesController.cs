using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingSystem.Data;
using TrainingSystem.Models;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/Courses")]
    public class CoursesController : Controller
    {
        private readonly TrainingSystemContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CoursesController(TrainingSystemContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Courses
        [HttpGet]
        public IEnumerable<CourseViewModel> GetCourses()
        {
            var courses = _context.Course
                .Include(c => c.Lessons)
                    .ThenInclude(l => l.Videos)
                .Include(c => c.Ratings)
                .ToList();

            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.FindByEmailAsync(email).Result;

            var subscriptions = new List<int>();
            if (_userManager.IsInRoleAsync(user, "Employee").Result)
            {
                var employee = _context.Employee.SingleOrDefault(e => e.AppUserId == user.Id);
                subscriptions = _context.CourseSubscription
                                            .Where(cs => cs.EmployeeId == employee.EmployeeId)
                                            .Select(cs => cs.CourseId)
                                            .ToList();
            }

            var vms = new List<CourseViewModel>();
            courses.ForEach(c => {
                var vm = new CourseViewModel(c);
                vm.IsSubscribed = subscriptions.Contains(c.CourseId);
                vms.Add(vm);
            });

            return vms;
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var course = await _context.Course
                .Include(c => c.Lessons)
                    .ThenInclude(l => l.Videos)
                .Include(c => c.Lessons)
                    .ThenInclude(l => l.Exam)
                .Include(c => c.Materials)
                .Include(c => c.Keywords)
                .Include(c => c.Ratings)
                .SingleOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.FindByEmailAsync(email).Result;

            if(_userManager.IsInRoleAsync(user, "Admin").Result) {
                return Ok(course);
            }

            var employee = _context.Employee.SingleOrDefault(e => e.AppUserId == user.Id);
            var isSubscribed = _context.CourseSubscription
                                       .Any(cs => cs.CourseId == id && cs.EmployeeId == employee.EmployeeId);
            
            
            var vm = new CourseViewModel(course);
            vm.IsSubscribed = isSubscribed;

            if(!isSubscribed) {
                // If user is not subscribed dont send him file info
                vm.Materials.ForEach(m => m.FileName = null);
                vm.Lessons.ForEach(l => l.Videos.ForEach(v => v.FileName = null));
            }

            return Ok(vm);
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse([FromRoute] int id, [FromBody] Course newCourse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != newCourse.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(newCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        [HttpPost]
        public async Task<IActionResult> PostCourse([FromBody] Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = await _context.Course.SingleOrDefaultAsync(c => c.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return Ok(course);
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(c => c.CourseId == id);
        }
    }
}