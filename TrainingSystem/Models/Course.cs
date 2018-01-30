using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainingSystem.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Instructor { get; set; }
        public string Description { get; set; }

        public List<Keyword> Keywords { get; set; }
        public List<Lesson> Lessons { get; set; }
        public List<Material> Materials { get; set; }
        public List<Rating> Ratings { get; set; }
    }
}
