namespace CarRentalSystem.Application.Features.CarAds.Queries.Search;

using System;

public class CarAdListingModel
{
    public CarAdListingModel(
        Guid id, 
        string manufacturer,
        string model,
        string imageUrl,
        string category, 
        decimal pricePerDay)
    {
        this.Id = id;
        this.Manufacturer = manufacturer;
        this.Model = model;
        this.ImageUrl = imageUrl;
        this.Category = category;
        this.PricePerDay = pricePerDay;
    }

    public Guid Id { get; }

    public string Manufacturer { get; }

    public string Model { get; }

    public string ImageUrl { get; }

    public string Category { get; }

    public decimal PricePerDay { get; }
}
