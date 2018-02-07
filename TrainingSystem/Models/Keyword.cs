using System.ComponentModel.DataAnnotations;

namespace TrainingSystem.Models
{
    public class Keyword
    {
        public int KeywordId { get; set; }
        [Required]
        public string Name { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
