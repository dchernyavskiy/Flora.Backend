using Flora.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flora.Infrastructure.Persistence.Configurations;

public class CharacteristicValueConfiguration : IEntityTypeConfiguration<CharacteristicValue>
{
    public void Configure(EntityTypeBuilder<CharacteristicValue> builder)
    {
        builder.HasOne(x => x.Characteristic)
            .WithMany(x => x.CharacteristicValues)
            .HasForeignKey(x => x.CharacteristicId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Plant)
            .WithMany(x => x.CharacteristicValues)
            .HasForeignKey(x => x.PlantId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}