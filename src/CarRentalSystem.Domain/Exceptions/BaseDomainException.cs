namespace CarRentalSystem.Domain.Exceptions;

using System;

public abstract class BaseDomainException : Exception
{
    private string? message;

    public override string Message => this.message ?? base.Message;

    public string Error
    {
        set => this.message = value;
    }
}
