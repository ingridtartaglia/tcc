using System.Collections.Generic;

namespace TrainingSystem.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public string Name { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public Exam Exam { get; set; }
        public List<Video> Videos { get; set; }
    }
}
