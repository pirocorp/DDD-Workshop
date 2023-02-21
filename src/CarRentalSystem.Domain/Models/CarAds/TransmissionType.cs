namespace CarRentalSystem.Domain.Models.CarAds;

using CarRentalSystem.Domain.Common;

public class TransmissionType : Enumeration
{
    public static readonly TransmissionType Manual = new (1, nameof(Manual));
    public static readonly TransmissionType Automatic = new (2, nameof(Automatic));

    private TransmissionType(int value) // EF Required
        : this(value, FromValue<TransmissionType>(value).Name)
    { }

    private TransmissionType(int value, string name) 
        : base(value, name)
    { }
}
