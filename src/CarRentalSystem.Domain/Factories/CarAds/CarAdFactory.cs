namespace CarRentalSystem.Domain.Factories.CarAds;

using System.Diagnostics.CodeAnalysis;

using CarRentalSystem.Domain.Exceptions;
using CarRentalSystem.Domain.Models.CarAds;

internal class CarAdFactory : ICarAdFactory
{
    private string carAdModel = string.Empty;
    private string carAdImageUrl = string.Empty;
    private decimal carAdPricePerDay = default;

    private Category? carAdCategory = null;
    private Manufacturer? carAdManufacturer = null;
    private Options? carAdOptions = null;

    [MemberNotNullWhen(returnValue: true, nameof(carAdCategory))]
    private bool CategorySet => this.carAdCategory is not null;

    [MemberNotNullWhen(returnValue: true, nameof(carAdManufacturer))]
    private bool ManufacturerSet => this.carAdManufacturer is not null;

    [MemberNotNullWhen(returnValue: true, nameof(carAdOptions))]
    private bool OptionsSet => this.carAdOptions is not null;

    public ICarAdFactory WithManufacturer(string name)
        => this.WithManufacturer(new Manufacturer(name));

    public ICarAdFactory WithManufacturer(Manufacturer? manufacturer)
    {
        this.carAdManufacturer = manufacturer;
        return this;
    }

    public ICarAdFactory WithModel(string model)
    {
        this.carAdModel = model;
        return this;
    }

    public ICarAdFactory WithCategory(string name, string description)
        => this.WithCategory(new Category(name, description));

    public ICarAdFactory WithCategory(Category? category)
    {
        this.carAdCategory = category;
        return this;
    }

    public ICarAdFactory WithImageUrl(string imageUrl)
    {
        this.carAdImageUrl = imageUrl;
        return this;
    }

    public ICarAdFactory WithPricePerDay(decimal pricePerDay)
    {
        this.carAdPricePerDay = pricePerDay;
        return this;
    }

    public ICarAdFactory WithOptions(bool hasClimateControl, int numberOfSeats, TransmissionType transmissionType)
        => this.WithOptions(new Options(hasClimateControl, numberOfSeats, transmissionType));

    public ICarAdFactory WithOptions(Options? options)
    {
        this.carAdOptions = options;
        return this;
    }

    public CarAd Build()
    {
        if (!this.ManufacturerSet || !this.CategorySet || !this.OptionsSet)
        {
            throw new InvalidCarAdException("Manufacturer, category and options must have a value.");
        }

        return new CarAd(
            this.carAdManufacturer,
            this.carAdModel,
            this.carAdCategory,
            this.carAdImageUrl,
            this.carAdPricePerDay,
            this.carAdOptions,
            true);
    }
}
