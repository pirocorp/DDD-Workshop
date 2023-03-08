namespace CarRentalSystem.Application.Features.Identity;

using CarRentalSystem.Domain.Models.Dealers;

public interface IUser
{
    void BecomeDealer(Dealer dealer);
}
