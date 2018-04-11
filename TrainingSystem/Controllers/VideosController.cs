using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingSystem.Data;
using TrainingSystem.Models;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/Videos")]
    public class VideosController : Controller
    {
        private readonly TrainingSystemContext _context;
        private readonly IHostingEnvironment _environment;

        public VideosController(TrainingSystemContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // POST: api/Videos
        [HttpPost]
        public async Task<IActionResult> PostVideo([FromForm] VideoViewModel videoVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var filePath = Path.Combine(_environment.ContentRootPath, @"Uploads", videoVm.File.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await videoVm.File.CopyToAsync(stream);
            }

            var video = new Video {
                Name = videoVm.Name,
                FileName = videoVm.File.FileName,
                LessonId = videoVm.LessonId
            };

            _context.Video.Add(video);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVideo", new { id = video.VideoId }, video);
        }

        // DELETE: api/Videos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var video = await _context.Video.SingleOrDefaultAsync(v => v.VideoId == id);
            if (video == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_environment.ContentRootPath, @"Uploads", video.FileName);
            System.IO.File.Delete(filePath);

            _context.Video.Remove(video);
            await _context.SaveChangesAsync();

            return Ok(video);
        }

        private bool VideoExists(int id)
        {
            return _context.Video.Any(v => v.VideoId == id);
        }
    }
}