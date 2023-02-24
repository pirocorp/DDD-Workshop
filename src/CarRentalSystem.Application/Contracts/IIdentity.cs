namespace CarRentalSystem.Application.Contracts;

using CarRentalSystem.Application.Common;
using CarRentalSystem.Application.Features.Identity;
using CarRentalSystem.Application.Features.Identity.Commands.LoginUser;

public interface IIdentity
{
    Task<Result> Register(UserInputModel userInput);

    Task<Result<LoginOutputModel>> Login(UserInputModel userInput);
}
