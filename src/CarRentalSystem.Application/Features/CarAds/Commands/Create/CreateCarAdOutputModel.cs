namespace CarRentalSystem.Application.Features.CarAds.Commands.Create;

using System;

public class CreateCarAdOutputModel
{
    public CreateCarAdOutputModel(Guid carAdId)
    {
        this.CarAdId = carAdId;
    }

    public Guid CarAdId { get; }
}
