using dotnet_exemplar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dotnet_exemplar.Infrastructure.Data.Configurations;

public class ApiTokenConfiguration : IEntityTypeConfiguration<ApiToken>
{
    public void Configure(EntityTypeBuilder<ApiToken> builder)
    {
        builder.Property(t => t.Key)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Email)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(t => t.Key).IsUnique();
    }
}