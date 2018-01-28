using System.ComponentModel.DataAnnotations;

namespace TrainingSystem.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }
        [Required]
        public string AnswerName { get; set; }
        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
