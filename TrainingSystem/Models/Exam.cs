using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainingSystem.Models
{
    public class Exam
    {
        public int ExamId { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        [Required]
        public List<Question> Questions { get; set; }
    }
}
