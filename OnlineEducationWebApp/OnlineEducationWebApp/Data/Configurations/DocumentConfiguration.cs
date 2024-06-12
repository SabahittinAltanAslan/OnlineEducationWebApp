using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineEducationWebApp.Data.Entities;

namespace OnlineEducationWebApp.Data.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.Property(x => x.OriginalFileName).IsRequired();
            builder.Property(x=>x.Description).HasMaxLength(150);
            builder.Property(x=>x.FilePath).HasMaxLength(250);
        }
    }
}
