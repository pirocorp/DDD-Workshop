namespace CarRentalSystem.Application.Features.Identity;

using System.Threading.Tasks;

using CarRentalSystem.Application.Common;
using CarRentalSystem.Application.Features.Identity.Commands.LoginUser;

public interface IIdentity
{
    Task<Result<IUser>> Register(UserInputModel userInput);

    Task<Result<LoginOutputModel>> Login(UserInputModel userInput);
}
