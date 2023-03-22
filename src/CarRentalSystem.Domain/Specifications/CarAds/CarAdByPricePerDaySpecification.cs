namespace CarRentalSystem.Domain.Specifications.CarAds;

using System;
using System.Linq.Expressions;

using CarRentalSystem.Domain.Models.CarAds;

/// <summary>
/// Excluding price range
/// </summary>
public class CarAdByPricePerDaySpecification : Specification<CarAd>
{
    private readonly decimal minPrice;
    private readonly decimal maxPrice;

    public CarAdByPricePerDaySpecification(
        decimal? minPrice = default, 
        decimal? maxPrice = decimal.MaxValue)
    {
        this.minPrice = minPrice ?? default;
        this.maxPrice = maxPrice ?? int.MaxValue;
    }

    public override Expression<Func<CarAd, bool>> ToExpression()
        => carAd => this.minPrice < carAd.PricePerDay && carAd.PricePerDay < this.maxPrice;
}
