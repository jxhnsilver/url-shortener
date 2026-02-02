using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Infrastructure.Configurations
{
    public class ShortUrlConfiguration : IEntityTypeConfiguration<ShortUrl>
    {
        public void Configure(EntityTypeBuilder<ShortUrl> builder)
        {
            builder.ToTable("short_url");

            builder.HasKey(s => s.Id)
                .HasName("id");

            builder.Property(s => s.OriginalUrl)
                .HasColumnName("original_url")
                .IsRequired()
                .HasMaxLength(2048);

            builder.Property(s => s.ShortCode)
                .HasColumnName("short_code")
                .IsRequired()
                .HasMaxLength(16);

            builder.HasIndex(s => s.OriginalUrl).IsUnique();
            builder.HasIndex(s => s.ShortCode).IsUnique();
        }
    }
}
