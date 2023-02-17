﻿namespace CarRentalSystem.Domain.Models.CarAds;

using System;

using CarRentalSystem.Domain.Common;
using CarRentalSystem.Domain.Exceptions;

using static CarRentalSystem.Domain.Models.ModelConstants.CarAd;

public class CarAd : Entity<Guid>, IAggregateRoot
{
    public CarAd(
        Manufacturer manufacturer, 
        string model, 
        Category category, 
        string imageUrl,
        decimal pricePerDay, 
        Options options, 
        bool isAvailable,
        TransmissionType transmissionType)
    {
        this.Validate(model, imageUrl, pricePerDay);

        this.Id = Guid.NewGuid();

        this.Manufacturer = manufacturer;
        this.Model = model;
        this.Category = category;
        this.ImageUrl = imageUrl;
        this.PricePerDay = pricePerDay;
        this.Options = options;
        this.IsAvailable = isAvailable;
        this.TransmissionType = transmissionType;
    }

    public Manufacturer Manufacturer { get; }

    public string Model { get; }

    public Category Category { get; }

    public string ImageUrl { get; }

    public decimal PricePerDay { get; }

    public Options Options { get; }

    public bool IsAvailable { get; private set; }

    public TransmissionType TransmissionType { get; }

    public void ChangeAvailability() => this.IsAvailable = !this.IsAvailable;

    private void Validate(string model, string imageUrl, decimal pricePerDay)
    {
        Guard.ForStringLength<InvalidCarAdException>(
            model,
            MIN_MODEL_LENGTH,
            MAX_MODEL_LENGTH,
            nameof(this.Model));

        Guard.ForValidUrl<InvalidCarAdException>(
            imageUrl,
            nameof(this.ImageUrl));

        Guard.AgainstOutOfRange<InvalidCarAdException>(
            pricePerDay,
            decimal.Zero, 
            decimal.MaxValue,
            nameof(this.PricePerDay));
    }
}
