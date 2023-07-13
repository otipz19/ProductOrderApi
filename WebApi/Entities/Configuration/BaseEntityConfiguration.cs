using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Entities.Configuration
{
    internal class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
        where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(nameof(BaseEntity.CreatedAt))
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
