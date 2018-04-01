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
    [Route("api/VideoWatch")]
    public class VideoWatchController : Controller
    {
        private readonly TrainingSystemContext _context;
        private readonly UserManager<AppUser> _userManager;

        public VideoWatchController(TrainingSystemContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: api/VideoWatch/5
        [HttpPost]
        public async Task<IActionResult> PostUserVideoWatch([FromBody] int id)
        {
            string email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userManager.FindByEmailAsync(email).Result;
            var employee = _context.Employee.SingleOrDefault(e => e.AppUserId == user.Id);

            var videoWatch = new VideoWatch();
            videoWatch.VideoId = id;
            videoWatch.EmployeeId = employee.EmployeeId;

            _context.VideoWatch.Add(videoWatch);
            await _context.SaveChangesAsync();

            return Ok(videoWatch);
        }

        // PUT: api/VideoWatch
        [HttpPut]
        public async Task<IActionResult> PutUserVideoWatch([FromBody] VideoWatch videoWatch)
        {
            _context.Entry(videoWatch).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!VideoWatchExists(videoWatch)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        private bool VideoWatchExists(VideoWatch videoWatch) {
            return _context.VideoWatch
                .Any(v => v.VideoId == videoWatch.VideoId && v.EmployeeId == videoWatch.EmployeeId);
        }
    }
}