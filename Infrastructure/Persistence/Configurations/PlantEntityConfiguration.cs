using Flora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flora.Infrastructure.Persistence.Configurations;

public class PlantEntityConfiguration : IEntityTypeConfiguration<Plant>
{
    public void Configure(EntityTypeBuilder<Plant> builder)
    {
        builder.HasMany(x => x.CharacteristicValues)
            .WithOne(x => x.Plant)
            .HasForeignKey(x => x.PlantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}