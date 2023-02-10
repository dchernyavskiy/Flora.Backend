using FluentValidation;

namespace Flora.Application.Plants.Queries.GetPlant;

public class GetPlantQueryValidator : AbstractValidator<GetPlantQuery>
{
    public GetPlantQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}