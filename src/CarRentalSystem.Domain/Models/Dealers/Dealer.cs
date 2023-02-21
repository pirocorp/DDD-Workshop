namespace CarRentalSystem.Domain.Models.Dealers;

using System;
using System.Collections.Generic;
using System.Linq;

using CarRentalSystem.Domain.Common;
using CarRentalSystem.Domain.Exceptions;
using CarRentalSystem.Domain.Models.CarAds;

using static CarRentalSystem.Domain.Models.ModelConstants.Common;

public class Dealer : Entity<Guid>, IAggregateRoot
{
    private readonly HashSet<CarAd> carAds;

    public Dealer(string name, string phoneNumber)
    {
        this.Validate(name);

        this.Id = Guid.NewGuid();

        this.Name = name;
        this.PhoneNumber = phoneNumber;

        this.carAds = new HashSet<CarAd>();
    }

    // EF  Required constructors that bind non-navigational properties
    private Dealer(string name)
    {
        this.Name = name;

        this.PhoneNumber = default!;
        this.carAds = new HashSet<CarAd>();
    }

    public string Name { get; set; }

    public PhoneNumber PhoneNumber { get; set; }

    public IReadOnlyCollection<CarAd> CarAds => this.carAds.ToList().AsReadOnly();

    public void AddCarAd(CarAd carAd) => this.carAds.Add(carAd);

    private void Validate(string name)
        => Guard.ForStringLength<InvalidDealerException>(
            name,
            MIN_NAME_LENGTH,
            MAX_NAME_LENGTH,
            nameof(this.Name));
}
