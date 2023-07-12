using AutoMapper;
using BuildingBlocks.Abstractions.Domain;
using BuildingBlocks.Abstractions.Mapping;
using Flora.Services.Catalogs.Characteristics.Models;

namespace Flora.Services.Catalogs.Characteristics.Dtos;

public class CharacteristicDto : IMapWith<CharacteristicValue>, IHaveIdentity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CharacteristicValue, CharacteristicDto>()
            .ForMember(x => x.Name, opts => opts.MapFrom(src => src.Characteristic.Name));
    }
}
