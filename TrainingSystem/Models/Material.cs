using System.ComponentModel.DataAnnotations;

namespace TrainingSystem.Models
{
    public class Material
    {
        public int MaterialId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FileName { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
