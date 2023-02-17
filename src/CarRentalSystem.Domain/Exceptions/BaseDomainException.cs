namespace CarRentalSystem.Domain.Exceptions;

using System;

public abstract class BaseDomainException : Exception
{
    private string? error;

    public new string Error
    {
        get => this.error ?? base.Message;
        set => this.error = value;
    }
}
