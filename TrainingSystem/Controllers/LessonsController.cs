using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingSystem.Data;
using TrainingSystem.Models;

namespace TrainingSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/Lessons")]
    public class LessonsController : Controller
    {
        private readonly TrainingSystemContext _context;

        public LessonsController(TrainingSystemContext context)
        {
            _context = context;
        }

        // GET: api/Lessons/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLesson([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lesson = await _context.Lesson
                .Include(l => l.Course)
                .Include(l => l.Videos)
                .Include(l => l.Exam)
                .ThenInclude(e => e.Questions)
                .ThenInclude(q => q.QuestionChoices)
                .SingleOrDefaultAsync(l => l.LessonId == id);

            if (lesson == null)
            {
                return NotFound();
            }

            return Ok(lesson);
        }

        // POST: api/Lessons
        [HttpPost]
        public async Task<IActionResult> PostLesson([FromBody] Lesson lesson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Lesson.Add(lesson);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLesson", new { id = lesson.LessonId }, lesson);
        }

        // DELETE: api/Lessons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lesson = await _context.Lesson.SingleOrDefaultAsync(l => l.LessonId == id);
            if (lesson == null)
            {
                return NotFound();
            }

            _context.Lesson.Remove(lesson);
            await _context.SaveChangesAsync();

            return Ok(lesson);
        }

        private bool LessonExists(int id)
        {
            return _context.Lesson.Any(l => l.LessonId == id);
        }
    }
}