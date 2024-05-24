using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineEducationWebApp.Data.Entities;

namespace OnlineEducationWebApp.Data.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Url).HasMaxLength(100);
        }
    }
}
