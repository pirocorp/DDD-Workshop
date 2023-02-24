namespace CarRentalSystem.Application.Features.Identity.Commands.CreateUser;

using CarRentalSystem.Application.Common;
using CarRentalSystem.Application.Contracts;

using MediatR;

public class CreateUserCommand : UserInputModel, IRequest<Result>
{
    public CreateUserCommand(string email, string password) 
        : base(email, password)
    {
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
    {
        private readonly IIdentity identity;

        public CreateUserCommandHandler(IIdentity identity)
        {
            this.identity = identity;
        }

        public Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            => this.identity.Register(request);
    }
}
