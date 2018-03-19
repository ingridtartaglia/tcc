using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainingSystem.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        [Required]
        public string QuestionName { get; set; }

        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public List<QuestionChoice> QuestionChoices { get; set; }
    }
}