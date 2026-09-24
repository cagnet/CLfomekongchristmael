using CustomerOrders.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerOrders.Infrastructure.Persistence.Database.Configurations;

public class OrderConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders", table =>
        {
            table.HasCheckConstraint(
                "CK_Orders_Amount", "Amount >= 0.01 AND Amount <= 1000000000");
        });
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Amount)
            .HasPrecision(12, 2)
            .IsRequired();

        builder.HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId);
    }
}