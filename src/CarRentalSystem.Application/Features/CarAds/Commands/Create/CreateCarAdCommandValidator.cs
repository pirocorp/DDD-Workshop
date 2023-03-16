namespace CarRentalSystem.Application.Features.CarAds.Commands.Create;

using System;

using CarRentalSystem.Domain.Common;
using CarRentalSystem.Domain.Models.CarAds;

using FluentValidation;

using static Domain.Models.ModelConstants.CarAd;
using static Domain.Models.ModelConstants.Common;
using static Domain.Models.ModelConstants.Options;

public class CreateCarAdCommandValidator : AbstractValidator<CreateCarAdCommand>
{
    public CreateCarAdCommandValidator(ICarAdRepository carAdRepository)
    {
        this.RuleFor(c => c.Manufacturer)
            .MinimumLength(MIN_NAME_LENGTH)
            .MaximumLength(MAX_NAME_LENGTH)
            .NotEmpty()
            .WithMessage($"'{{PropertyName}}' should be with length between ${MIN_NAME_LENGTH} and ${MAX_NAME_LENGTH}.");

        this.RuleFor(c => c.Model)
            .MinimumLength(MIN_MODEL_LENGTH)
            .MaximumLength(MAX_MODEL_LENGTH)
            .NotEmpty()
            .WithMessage($"'{{PropertyName}}' should be with length between ${MIN_MODEL_LENGTH} and ${MAX_MODEL_LENGTH}.");;

        this.RuleFor(c => c.Category)
            .MustAsync(async (category, token) => await carAdRepository
                .GetCategory(category, token) != null)
            .WithMessage("'{PropertyName}' does not exist.");

        this.RuleFor(c => c.ImageUrl)
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            .WithMessage("'{PropertyName}' must be a valid url.")
            .NotEmpty();

        this.RuleFor(c => c.PricePerDay)
            .InclusiveBetween(decimal.Zero, decimal.MaxValue)
            .WithMessage($"'{{PropertyName}}' should be between ${decimal.Zero} and ${decimal.MaxValue}.");

        this.RuleFor(c => c.NumberOfSeats)
            .InclusiveBetween(MIN_NUMBER_OF_SEATS, MAX_NUMBER_OF_SEATS)
            .WithMessage($"'{{PropertyName}}' should be between ${MIN_NUMBER_OF_SEATS} and ${MAX_NUMBER_OF_SEATS}.");

        this.RuleFor(c => c.TransmissionType)
            .Must(BeAValidTransmissionType)
            .WithMessage("'{PropertyName}' is not a valid transmission type.");
    }

    private static bool BeAValidTransmissionType(int transmissionType)
    {
        // TODO: Add TryParse<TEnum> and use it instead.

        try
        {
            Enumeration.FromValue<TransmissionType>(transmissionType);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
