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
    [Route("api/QuestionChoice")]
    public class QuestionChoicesController : Controller
    {
        private readonly TrainingSystemContext _context;

        public QuestionChoicesController(TrainingSystemContext context)
        {
            _context = context;
        }

        // GET: api/QuestionChoices
        [HttpGet]
        public IEnumerable<QuestionChoice> GetQuestionChoices()
        {
            return _context.QuestionChoice;
        }

        // GET: api/QuestionChoices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionChoice([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questionChoice = await _context.QuestionChoice.SingleOrDefaultAsync(q => q.QuestionChoiceId == id);

            if (questionChoice == null)
            {
                return NotFound();
            }

            return Ok(questionChoice);
        }

        // PUT: api/QuestionChoices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestionChoice([FromRoute] int id, [FromBody] QuestionChoice questionChoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != questionChoice.QuestionChoiceId)
            {
                return BadRequest();
            }

            _context.Entry(questionChoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionChoiceExists(id))
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

        // POST: api/QuestionChoices
        [HttpPost]
        public async Task<IActionResult> PostQuestionChoice([FromBody] QuestionChoice questionChoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.QuestionChoice.Add(questionChoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestionChoice", new { id = questionChoice.QuestionChoiceId }, questionChoice);
        }

        // DELETE: api/QuestionChoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionChoice([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var questionChoice = await _context.QuestionChoice.SingleOrDefaultAsync(q => q.QuestionChoiceId == id);
            if (questionChoice == null)
            {
                return NotFound();
            }

            _context.QuestionChoice.Remove(questionChoice);
            await _context.SaveChangesAsync();

            return Ok(questionChoice);
        }

        private bool QuestionChoiceExists(int id)
        {
            return _context.QuestionChoice.Any(e => e.QuestionChoiceId == id);
        }
    }
}