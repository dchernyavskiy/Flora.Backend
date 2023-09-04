using Flora.Services.Orders.Baskets.Models;
using Flora.Services.Orders.Shared.Data;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flora.Services.Orders.Baskets.Data;

public class BasketEntityTypeConfiguration : IEntityTypeConfiguration<Basket>
{
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
        builder.ToTable(nameof(Basket).Pluralize().Underscore(), OrderDbContext.DefaultSchema);

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();

        builder.HasMany(x => x.BasketItems)
            .WithOne(x => x.Basket)
            .HasForeignKey(x => x.BasketId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class BasketItemEntityTypeConfiguration : IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        builder.ToTable(nameof(BasketItem).Pluralize().Underscore(), OrderDbContext.DefaultSchema);

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();

        builder.HasOne(x => x.Product)
            .WithMany(x => x.BasketItems)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
