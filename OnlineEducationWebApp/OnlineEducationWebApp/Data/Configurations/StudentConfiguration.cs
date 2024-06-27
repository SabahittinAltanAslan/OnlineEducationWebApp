using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineEducationWebApp.Data.Entities;

namespace OnlineEducationWebApp.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Surname).IsRequired();
            builder.Property(x => x.SchNumber).IsRequired();
            builder.Property(x => x.BirthDay).IsRequired();
            builder.Property(x=>x.SchNumber).HasMaxLength(10);
            builder.Property(x=>x.Email).IsRequired();
            builder.Property(x=>x.Email).HasMaxLength(10);
            builder.Property(x=>x.Password).IsRequired();
        }
    }
}