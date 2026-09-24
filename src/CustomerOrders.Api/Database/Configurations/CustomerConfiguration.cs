using CustomerOrders.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerOrders.Api.Database.Configurations;

public class CustomerConfiguration: IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");
        builder.HasKey(c => c.Id);
        builder.Property(p => p.Email).IsRequired()
            .HasMaxLength(150);
        builder.Property(p => p.Name)
            .IsRequired().HasMaxLength(30);
        builder.Property(p => p.FirstName)
            .IsRequired().HasMaxLength(20);
        builder.Property(p => p.Address)
            .IsRequired().HasMaxLength(20);

        builder.HasIndex(p => p.Email)
            .IsUnique();
    }
}