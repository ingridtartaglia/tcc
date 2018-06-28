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
                .Include(c => c.Lessons).ThenInclude(l => l.Videos)
                .Include(c => c.Lessons).ThenInclude(l => l.Exam)
                .Include(c => c.Ratings)
                .ToList();

            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.FindByEmailAsync(email).Result;

            var subscriptions = new List<int>();
            var vms = new List<CourseViewModel>();
            if (_userManager.IsInRoleAsync(user, "Employee").Result)
            {
                var employee = _context.Employee.SingleOrDefault(e => e.AppUserId == user.Id);
                subscriptions = _context.CourseSubscription
                                            .Where(cs => cs.EmployeeId == employee.EmployeeId)
                                            .Select(cs => cs.CourseId)
                                            .ToList();

                courses.ForEach(c => {
                    var vm = new CourseViewModel(c);
                    vm.IsSubscribed = subscriptions.Contains(c.CourseId);
                    vms.Add(vm);
                });
            }
            else
            {
                foreach(var course in courses)
                {
                    var subscribedUsers = _context.CourseSubscription
                                                  .Where(cs => cs.CourseId == course.CourseId)
                                                  .Select(cs => cs.EmployeeId);
                    var videos = course.Lessons.SelectMany(l => l.Videos).Select(v => v.VideoId);
                    var exams = course.Lessons.Where(l=> l.Exam != null).Select(l => l.Exam).Select(e => e.ExamId);

                    int finishedVideosUserCount = 0;
                    int finishedExamsUserCount = 0;

                    foreach (var u in subscribedUsers)
                    {
                        var videosWatch = _context.VideoWatch
                                                  .Where(vw => vw.EmployeeId == u && videos.Contains(vw.VideoId) && vw.IsCompleted);
                        if (videosWatch.Count() == videos.Count()) finishedVideosUserCount++;

                        var userExams = _context.UserExam
                                                .Where(ue => ue.EmployeeId == u && exams.Contains(ue.ExamId) && ue.IsApproved);
                        if (userExams.Count() == exams.Count()) finishedExamsUserCount++;
                    }

                    var vm = new CourseViewModel(course);
                    vm.SubscribedUsers = subscribedUsers.Count();
                    vm.FinishedVideosUserCount = finishedVideosUserCount;
                    vm.FinishedExamsUserCount = finishedExamsUserCount;
                    vms.Add(vm);
                }
            }
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
                                       .Any(cs => cs.CourseId == id && 
                                                  cs.EmployeeId == employee.EmployeeId);
            var videoIds = course.Lessons.SelectMany(l=> l.Videos.Select(v => v.VideoId)).ToList();
            var videoWatch = _context.VideoWatch
                                     .Where(vw => vw.EmployeeId == employee.EmployeeId &&
                                                  videoIds.Contains(vw.VideoId))
                                     .ToList();
            var examIds = course.Lessons.Where(l => l.Exam != null).Select(l => l.Exam.ExamId).ToList();
            var userExams = _context.UserExam
                                    .Where(ue => ue.EmployeeId == employee.EmployeeId &&
                                                 examIds.Contains(ue.ExamId))
                                    .ToList();

            var vm = new CourseViewModel(course);
            vm.IsSubscribed = isSubscribed;
            vm.VideoWatch = videoWatch;
            vm.UserExams = userExams;
            vm.AppUser = user;
            
            if(!isSubscribed) {
                // If user is not subscribed dont send him file info
                vm.Materials.ForEach(m => m.FileName = null);
                vm.Lessons.ForEach(l => l.Videos.ForEach(v => v.FileName = null));
            }

            return Ok(vm);
        }

        // GET: api/Courses/User/5
        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetUserCourses([FromRoute] int id)
        {
            var courseSubscriptions = _context.CourseSubscription
                .Where(cs => cs.EmployeeId == id);

            var courses = new List<CourseViewModel>();

            foreach (var courseSubscription in courseSubscriptions)
            {
                var course = await _context.Course
                    .Include(c => c.Lessons)
                        .ThenInclude(l => l.Videos)
                    .Include(c => c.Lessons)
                        .ThenInclude(l => l.Exam)
                    .Include(c => c.Materials)
                    .Include(c => c.Keywords)
                    .Include(c => c.Ratings)
                    .SingleOrDefaultAsync(c => c.CourseId == courseSubscription.CourseId);    

                var employee = _context.Employee.SingleOrDefault(e => e.EmployeeId == id);
                var videoIds = course.Lessons.SelectMany(l => l.Videos.Select(v => v.VideoId)).ToList();
                var videoWatch = _context.VideoWatch
                                        .Where(vw => vw.EmployeeId == employee.EmployeeId &&
                                                    videoIds.Contains(vw.VideoId))
                                        .ToList();
                var examIds = course.Lessons.Where(l => l.Exam != null).Select(l => l.Exam.ExamId).ToList();
                var userExams = _context.UserExam
                                        .Where(ue => ue.EmployeeId == employee.EmployeeId &&
                                                    examIds.Contains(ue.ExamId))
                                        .ToList();

                var vm = new CourseViewModel(course);
                vm.VideoWatch = videoWatch;
                vm.UserExams = userExams;
                courses.Add(vm);
            }

            return Ok(courses);
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