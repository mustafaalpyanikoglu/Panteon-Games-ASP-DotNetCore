﻿using FluentValidation;
using static Application.Features.BuildingConfigs.Constants.BuildingConfigMessages;

namespace Application.Features.BuildingConfigs.Commands.UpdateBuildingConfig;

public class UpdateBuildingConfigCommandValidator : AbstractValidator<UpdateBuildingConfigCommand>
{
    public UpdateBuildingConfigCommandValidator()
    {
        RuleFor(c => c.BuildingCost)
            .GreaterThan(0)
            .WithMessage(BuildingCostMustBeGreaterThanZero);

        RuleFor(c => c.ConstructionTime)
            .InclusiveBetween(30, 1800)
            .WithMessage(ConstructionTimeMustBeBetween30And1800Seconds);

        RuleFor(c => c.BuildingType)
            .IsInEnum()
            .WithMessage(BuildingTypeMustBeAValidEnumValue);
    }
}