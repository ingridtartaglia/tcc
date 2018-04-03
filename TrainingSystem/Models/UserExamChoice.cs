using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainingSystem.Models
{
    public class UserExamChoice
    {
        public int UserExamChoiceId { get; set; }
        public int QuestionChoiceId { get; set; }
        public QuestionChoice QuestionChoice { get; set; }
        public int? UserExamId { get; set; }
        public UserExam UserExam { get; set; }
    }
}