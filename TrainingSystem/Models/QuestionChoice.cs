using System.ComponentModel.DataAnnotations;

namespace TrainingSystem.Models
{
    public class QuestionChoice
    {
        public int QuestionChoiceId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
