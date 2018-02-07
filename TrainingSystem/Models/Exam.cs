using System.Collections.Generic;

namespace TrainingSystem.Models
{
    public class Exam
    {
        public int ExamId { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public List<Question> Questions { get; set; }
    }
}
