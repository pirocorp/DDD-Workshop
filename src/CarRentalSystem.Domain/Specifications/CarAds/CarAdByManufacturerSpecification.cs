﻿namespace CarRentalSystem.Domain.Specifications.CarAds;

using System;
using System.Linq.Expressions;

using CarRentalSystem.Domain.Models.CarAds;

public class CarAdByManufacturerSpecification : Specification<CarAd>
{
    private readonly string? manufacturer;

    public CarAdByManufacturerSpecification(string? manufacturer)
    {
        this.manufacturer = manufacturer;
    }

    protected override bool Include => this.manufacturer != null;

    public override Expression<Func<CarAd, bool>> ToExpression()
        => carAd => carAd.Manufacturer.Name.ToLower().Contains(this.manufacturer!.ToLower());
}
