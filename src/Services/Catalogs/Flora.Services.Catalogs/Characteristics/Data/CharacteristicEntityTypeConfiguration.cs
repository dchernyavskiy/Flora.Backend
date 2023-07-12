using Flora.Services.Catalogs.Characteristics.Models;
using Flora.Services.Catalogs.Shared.Data;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flora.Services.Catalogs.Characteristics.Data;

public class CharacteristicEntityTypeConfiguration : IEntityTypeConfiguration<Characteristic>
{
    public void Configure(EntityTypeBuilder<Characteristic> builder)
    {
        builder.ToTable(nameof(Characteristic).Pluralize().Underscore(), CatalogDbContext.DefaultSchema);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();

        builder.Property(x => x.Name).HasMaxLength(100);

        builder.HasMany(x => x.CharacteristicValues)
            .WithOne(x => x.Characteristic)
            .HasForeignKey(x => x.CharacteristicId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Category)
            .WithMany(x => x.Characteristics)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
