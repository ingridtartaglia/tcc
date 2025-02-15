﻿using System;
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
    [Route("api/Exams")]
    public class ExamsController : Controller
    {
        private readonly TrainingSystemContext _context;

        public ExamsController(TrainingSystemContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exam = await _context.Exam
                .Include(e => e.Lesson)
                .Include(e => e.Questions)
                    .ThenInclude(q => q.QuestionChoices)
                .SingleOrDefaultAsync(e => e.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            return Ok(exam);
        }

        // POST: api/Exams
        [HttpPost]
        public async Task<IActionResult> PostExam([FromBody] Exam exam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Exam.Add(exam);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExam", new { id = exam.ExamId }, exam);
        }

        // DELETE: api/Exams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exam = await _context.Exam.SingleOrDefaultAsync(e => e.ExamId == id);
            if (exam == null)
            {
                return NotFound();
            }

            _context.Exam.Remove(exam);
            await _context.SaveChangesAsync();

            return Ok(exam);
        }

        private bool ExamExists(int id)
        {
            return _context.Exam.Any(e => e.ExamId == id);
        }
    }
}