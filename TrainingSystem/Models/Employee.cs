using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingSystem.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        [Required]
        public string Occupation { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}