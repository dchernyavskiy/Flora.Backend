using System.Diagnostics;
using BuildingBlocks.Core.Persistence.EfCore;
using Flora.Services.Catalogs.Products.Models;
using Flora.Services.Catalogs.Shared.Data;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flora.Services.Catalogs.Products.Data;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(nameof(Product).Pluralize().Underscore(), CatalogDbContext.DefaultSchema);

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();


        builder
            .Property(x => x.ProductStatus)
            .HasDefaultValue(ProductStatus.Available)
            .HasMaxLength(EfConstants.Lenght.Short)
            .HasConversion(x => x.ToString(), x => (ProductStatus)Enum.Parse(typeof(ProductStatus), x));
        builder.OwnsOne(c => c.Stock);

        builder.OwnsMany(
            x => x.Images,
            x =>
            {
                x.WithOwner().HasForeignKey("owner_id");
                x.Property<Guid>("id");
                x.HasKey("id");
                x.HasIndex("id");
            });


        builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);
    }
}
