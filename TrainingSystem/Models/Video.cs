using System.ComponentModel.DataAnnotations;

namespace TrainingSystem.Models
{
    public class Video
    {
        public int VideoId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FileName { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
