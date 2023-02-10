using FluentValidation;

namespace Flora.Application.Plants.Queries.GetPlants;

public class GetPlantsQueryValidator : AbstractValidator<GetPlantsQuery>
{
    public GetPlantsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0);
        RuleFor(x => x.PageSize)
            .GreaterThan(0);
    }    
}