using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TrainingSystem.ViewModels
{
    public class VideoViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public int LessonId { get; set; }
    }
}