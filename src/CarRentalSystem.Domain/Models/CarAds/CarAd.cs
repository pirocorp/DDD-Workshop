﻿namespace CarRentalSystem.Domain.Models.CarAds;

using System;
using System.Collections.Generic;
using System.Linq;

using CarRentalSystem.Domain.Common;
using CarRentalSystem.Domain.Exceptions;

using static CarRentalSystem.Domain.Models.ModelConstants.CarAd;

public class CarAd : Entity<Guid>, IAggregateRoot
{
    private static readonly IEnumerable<Category> AllowedCategories
        = new CategoryData().GetData().Cast<Category>();

    internal CarAd(
        Manufacturer manufacturer, 
        string model, 
        Category category, 
        string imageUrl,
        decimal pricePerDay, 
        Options options, 
        bool isAvailable)
    {
        this.Validate(model, imageUrl, pricePerDay);
        this.ValidateCategory(category);

        this.Id = Guid.NewGuid();

        this.Manufacturer = manufacturer;
        this.Model = model;
        this.Category = category;
        this.ImageUrl = imageUrl;
        this.PricePerDay = pricePerDay;
        this.Options = options;
        this.IsAvailable = isAvailable;
    }

    // EF  Required constructors that bind non-navigational properties
    private CarAd(
        string model, 
        string imageUrl, 
        decimal pricePerDay, 
        bool isAvailable)
    {
        this.Model = model;
        this.ImageUrl = imageUrl;
        this.PricePerDay = pricePerDay;
        this.IsAvailable = isAvailable;

        this.Manufacturer = default!;
        this.Category = default!;
        this.Options = default!;
    }

    public Manufacturer Manufacturer { get; }

    public string Model { get; }

    public Category Category { get; }

    public string ImageUrl { get; }

    public decimal PricePerDay { get; }

    public Options Options { get; }

    public bool IsAvailable { get; private set; }

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

    private void ValidateCategory(Category category)
    {
        var categoryName = category.Name;

        if (AllowedCategories.Any(c => c.Name == categoryName))
        {
            return;
        }

        var allowedCategoryNames = string.Join(", ", AllowedCategories.Select(c => $"'{c.Name}'"));

        throw new InvalidCarAdException(
            $"'{categoryName}' is not a valid category. Allowed values are: {allowedCategoryNames}.");
    }
}
