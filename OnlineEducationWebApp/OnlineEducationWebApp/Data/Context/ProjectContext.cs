using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineEducationWebApp.Data.Configurations;
using OnlineEducationWebApp.Data.Entities;
using System.Reflection.Metadata;
using Document = OnlineEducationWebApp.Data.Entities.Document;

namespace OnlineEducationWebApp.Data.Context
{
    public class ProjectContext :DbContext
    {
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<StudentLesson> StudentLessons { get; set; }

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());
            modelBuilder.ApplyConfiguration(new LessonConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());

            /*A dependency such that a lesson can have more than one document,
            a lesson can only belong to one course.*/
            modelBuilder.Entity<Lesson>()
                .HasMany(x => x.Documents)
                .WithOne(x => x.Lesson)
                .HasForeignKey(x => x.LessonId);

            /*A dependency in which a student can have more than one course,
             * and a course can have more than one student.*/
            modelBuilder.Entity<StudentLesson>()
                .HasKey(x=> new {x.StudentId, x.LessonId});

            modelBuilder.Entity<StudentLesson>()
                .HasOne(x => x.Student)
                .WithMany(x=>x.StudentLessons)
                .HasForeignKey(x=>x.StudentId);

            modelBuilder.Entity<StudentLesson>()
                .HasOne(x => x.Lesson)
                .WithMany(x => x.StudentLessons)
                .HasForeignKey(x => x.LessonId);

            /*A dependency in which a course can have more than one teacher,
             * but a teacher can only teach one course.*/
            modelBuilder.Entity<Lesson>()
                .HasMany(x => x.Teachers)
                .WithOne(X => X.Lesson)
                .HasForeignKey(x=>x.LessonId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
