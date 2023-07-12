using BuildingBlocks.Abstractions.CQRS.Commands;

namespace Flora.Services.Catalogs.Products.Features.ChangingRestockThreshold.v1;

public record ChangeRestockThreshold(Guid ProductId, int NewRestockThreshold) : ITxCommand;
