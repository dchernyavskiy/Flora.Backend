using AutoMapper;
using Flora.Application.Characteristics.Commands.CreateCharacteristic;
using Flora.Application.Common.Interfaces;
using Flora.Application.Common.Mappings;
using Flora.Domain.Entities;
using MediatR;

namespace Flora.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand : IRequest<Guid>, IMapWith<Category>
{
    public string Name { get; set; }
    public Guid? ParentId { get; set; } = null;
    public ICollection<CreateCharacteristicCommand>? Characteristics { get; set; } = null;
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public CreateCategoryCommandHandler(IApplicationDbContext context, IMapper mapper, ISender sender)
    {
        _context = context;
        _mapper = mapper;
        _sender = sender;
    }

    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Category>(request);
        entity.Id = Guid.NewGuid();
        if (request.ParentId != Guid.Empty)
            entity.ParentId = request.ParentId;

        await _context.Categories.AddAsync(entity);

        if (request.Characteristics is not null)
            foreach (var characteristic in request.Characteristics)
            {
                await _sender.Send(characteristic with { CategoryId = entity.Id });
            }

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}