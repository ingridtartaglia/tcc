using Microsoft.EntityFrameworkCore;

namespace TrainingSystem.Data
{
    public class TrainingSystemContext : DbContext
    {
        public TrainingSystemContext(DbContextOptions<TrainingSystemContext> options) : base(options)
        {

        }
    }
}