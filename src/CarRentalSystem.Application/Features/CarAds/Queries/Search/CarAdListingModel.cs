namespace CarRentalSystem.Application.Features.CarAds.Queries.Search;

using System;

using AutoMapper;

using CarRentalSystem.Application.Mapping;
using CarRentalSystem.Domain.Models.CarAds;

public class CarAdListingModel : IMapFrom<CarAd>
{
    public Guid Id { get; private set; } = default;

    public string Manufacturer { get; private set; } = string.Empty;

    public string Model { get; private set; } = string.Empty;

    public string ImageUrl { get; private set; } = string.Empty;

    public string Category { get; private set; } = string.Empty;

    public decimal PricePerDay { get; private set; } = default;

    public void Mapping(Profile mapper)
        => mapper
            .CreateMap<CarAd, CarAdListingModel>()
            .ForMember(
                destination => destination.Manufacturer,
                cfg
                    => cfg.MapFrom(source => source.Manufacturer.Name))
            .ForMember(
                destination => destination.Category,
                cfg
                    => cfg.MapFrom(source => source.Category.Name));
}
