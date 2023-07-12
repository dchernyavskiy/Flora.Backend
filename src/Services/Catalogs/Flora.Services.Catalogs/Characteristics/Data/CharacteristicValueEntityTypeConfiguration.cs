using Flora.Services.Catalogs.Characteristics.Models;
using Flora.Services.Catalogs.Shared.Data;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Flora.Services.Catalogs.Characteristics.Data;

public class CharacteristicValueEntityTypeConfiguration : IEntityTypeConfiguration<CharacteristicValue>
{
    public void Configure(EntityTypeBuilder<CharacteristicValue> builder)
    {
        builder.ToTable(nameof(CharacteristicValue).Pluralize().Underscore(), CatalogDbContext.DefaultSchema);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();

        builder.Property(x => x.Value).HasMaxLength(100);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.CharacteristicValues)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Characteristic)
            .WithMany(x => x.CharacteristicValues)
            .HasForeignKey(x => x.CharacteristicId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
