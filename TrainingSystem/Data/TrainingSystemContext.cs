using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainingSystem.Models;

namespace TrainingSystem.Data
{
    public class TrainingSystemContext : IdentityDbContext<AppUser>
    {
        public TrainingSystemContext(DbContextOptions<TrainingSystemContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.Entity<CourseSubscription>().HasKey(a => new { a.CourseId, a.EmployeeId });
        }
        
        public DbSet<TrainingSystem.Models.Course> Course { get; set; }
        public DbSet<TrainingSystem.Models.CourseSubscription> CourseSubscription { get; set; }
        public DbSet<TrainingSystem.Models.QuestionChoice> QuestionChoice { get; set; }
        public DbSet<TrainingSystem.Models.Employee> Employee { get; set; }
        public DbSet<TrainingSystem.Models.Exam> Exam { get; set; }
        public DbSet<TrainingSystem.Models.Keyword> Keyword { get; set; }
        public DbSet<TrainingSystem.Models.Lesson> Lesson { get; set; }
        public DbSet<TrainingSystem.Models.Material> Material { get; set; }
        public DbSet<TrainingSystem.Models.Question> Question { get; set; }
        public DbSet<TrainingSystem.Models.Rating> Rating { get; set; }
        public DbSet<TrainingSystem.Models.Video> Video { get; set; }
    }
}