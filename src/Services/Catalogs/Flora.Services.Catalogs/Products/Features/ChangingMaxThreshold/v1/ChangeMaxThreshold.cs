using BuildingBlocks.Abstractions.CQRS.Commands;

namespace Flora.Services.Catalogs.Products.Features.ChangingMaxThreshold.v1;

public record ChangeMaxThreshold(long ProductId, int NewMaxThreshold) : ITxCommand;
