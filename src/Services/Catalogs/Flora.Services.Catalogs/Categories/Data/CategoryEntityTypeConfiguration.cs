using BuildingBlocks.Core.Persistence.EfCore;
using Flora.Services.Catalogs.Shared.Data;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flora.Services.Catalogs.Categories.Data;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(nameof(Category).Pluralize().Underscore(), CatalogDbContext.DefaultSchema);

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();

        builder.HasMany(x => x.Products)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Children)
            .WithOne(x => x.Parent)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(
            x => x.Image,
            x =>
            {
                x.Property(y => y.ImageUrl).HasColumnName("image_url");
                x.Property(y => y.IsMain).HasColumnName("is_main");
            });

        builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);
        builder.Property(x => x.Name).HasColumnType(EfConstants.ColumnTypes.NormalText).IsRequired();
    }
}
