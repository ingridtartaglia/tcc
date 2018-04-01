using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TrainingSystem.Models;

namespace TrainingSystem.ViewModels
{
    public class CourseViewModel
    {
        public CourseViewModel(Course course) {
            CourseId = course.CourseId;
            Category = course.Category;
            Name = course.Name;
            Instructor = course.Instructor;
            Description = course.Description;
            Keywords = course.Keywords;
            Materials = course.Materials;
            Lessons = course.Lessons;
            Ratings = course.Ratings;

            VideosCount = course.Lessons == null ? 0 : course.Lessons.Sum(l => l.Videos.Count());
            CourseRating = course.Ratings.Any() ? (double?)course.Ratings.Average(r => r.Grade) : null;
        }

        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Instructor { get; set; }
        public string Description { get; set; }

        public List<Keyword> Keywords { get; set; }
        public List<Lesson> Lessons { get; set; }
        public List<Material> Materials { get; set; }
        public List<Rating> Ratings { get; set; }
        public List<VideoWatch> VideoWatch { get; set; }
        public bool IsSubscribed { get; set; }
        public decimal? WatchedVideosPercentage { get; set; }
        public int VideosCount { get; set; }
        public double? CourseRating { get; set; }
        public bool IsCompleted { get; set; }
    }
}