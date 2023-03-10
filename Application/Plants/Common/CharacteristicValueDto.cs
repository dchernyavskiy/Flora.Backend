using AutoMapper;
using Flora.Application.Common.Mappings;
using Flora.Application.Common.Models;
using Flora.Domain.Entities;

namespace Flora.Application.Plants.Common;

public class CharacteristicValueDto : BaseDto, IMapWith<CharacteristicValue>
{
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CharacteristicValueDto, CharacteristicValue>()
            .ForMember(x => x.CharacteristicId, opts => opts.MapFrom(src => src.Id))
            .ForMember(x => x.Value, opts => opts.MapFrom(src => src.Value));

        profile.CreateMap<CharacteristicValue, CharacteristicValueDto>()
            .ForMember(x => x.Name, opts => opts.MapFrom(src => src.Characteristic.Name))
            .ForMember(x => x.Value, opts => opts.MapFrom(src => src.Value))
            .ForMember(x => x.Id, opts => opts.MapFrom(src => src.CharacteristicId));
    }
}