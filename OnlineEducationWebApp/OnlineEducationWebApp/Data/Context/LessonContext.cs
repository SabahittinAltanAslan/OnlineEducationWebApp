using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineEducationWebApp.Data.Configurations;
using OnlineEducationWebApp.Data.Entities;
using System.Reflection.Metadata;
using Document = OnlineEducationWebApp.Data.Entities.Document;

namespace OnlineEducationWebApp.Data.Context
{
    public class LessonContext :DbContext
    {
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Document> Documents { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("server=LEGEN\\MSSQLSERVER01;database=OnlineEdu;integrated security=true; Trust Server Certificate=true;");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());
            modelBuilder.ApplyConfiguration(new LessonConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
