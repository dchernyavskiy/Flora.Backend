﻿using FluentValidation;

namespace Flora.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(20)
            .NotEmpty();
    }
}