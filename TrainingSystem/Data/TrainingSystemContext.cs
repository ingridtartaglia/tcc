﻿using Microsoft.EntityFrameworkCore;
using TrainingSystem.Models;

namespace TrainingSystem.Data
{
    public class TrainingSystemContext : DbContext
    {
        public TrainingSystemContext(DbContextOptions<TrainingSystemContext> options) : base(options)
        {
        }
        public DbSet<TrainingSystem.Models.Course> Course { get; set; }
        public DbSet<TrainingSystem.Models.Answer> Answer { get; set; }
        public DbSet<TrainingSystem.Models.Exam> Exam { get; set; }
        public DbSet<TrainingSystem.Models.Keyword> Keyword { get; set; }
        public DbSet<TrainingSystem.Models.Lesson> Lesson { get; set; }
        public DbSet<TrainingSystem.Models.Material> Material { get; set; }
        public DbSet<TrainingSystem.Models.Question> Question { get; set; }
        public DbSet<TrainingSystem.Models.Rating> Rating { get; set; }
        public DbSet<TrainingSystem.Models.Video> Video { get; set; }
    }
}