using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainingSystem.Models
{
    public class UserExam
    {
        public int UserExamId { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public List<UserExamChoice> UserExamChoices { get; set; }
        public bool IsApproved { get; set; } 
    }
}