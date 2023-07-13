using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Entities.Configuration
{
    internal class OrderConfiguration : BaseEntityConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);
            builder.HasMany(order => order.Products)
                .WithMany(product => product.Orders)
                .UsingEntity<ProductInOrder>();
        }
    }
}
