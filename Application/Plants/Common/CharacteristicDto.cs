using AutoMapper;
using Flora.Application.Common.Mappings;
using Flora.Application.Common.Models;
using Flora.Domain.Entities;

namespace Flora.Application.Plants.Commands.CreatePlant;

public class CharacteristicDto : BaseDto, IMapWith<CharacteristicPlant>
{
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CharacteristicDto, CharacteristicPlant>()
            .ForMember(x => x.CharacteristicId, opts => opts.MapFrom(src => src.Id))
            .ForMember(x => x.Value, opts => opts.MapFrom(src => src.Value));

        profile.CreateMap<CharacteristicPlant, CharacteristicDto>()
            .ForMember(x => x.Name, opts => opts.MapFrom(src => src.Characteristic.Name))
            .ForMember(x => x.Value, opts => opts.MapFrom(src => src.Value))
            .ForMember(x => x.Id, opts => opts.MapFrom(src => src.CharacteristicId));
    }
}