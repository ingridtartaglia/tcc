using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingSystem.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int Grade { get; set; }
        public string Comment { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
