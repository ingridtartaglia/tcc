using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Threading.Tasks; 
 
namespace TrainingSystem.Models 
{ 
    public class VideoWatch 
    { 
        public int EmployeeId { get; set; } 
        public Employee Employee { get; set; } 
        public bool IsCompleted { get; set; } 
        public int VideoId { get; set; } 
        public Video Video { get; set; } 
    } 
} 