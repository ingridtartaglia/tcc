using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainingSystem.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }
        [Required]
        public string Name { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public Exam Exam { get; set; }
        public List<Video> Videos { get; set; }
    }
}
