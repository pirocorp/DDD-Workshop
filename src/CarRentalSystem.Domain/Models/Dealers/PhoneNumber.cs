namespace CarRentalSystem.Domain.Models.Dealers;

using System.Text.RegularExpressions;

using CarRentalSystem.Domain.Common;
using CarRentalSystem.Domain.Exceptions;

using static CarRentalSystem.Domain.Models.ModelConstants.PhoneNumber;

public class PhoneNumber : ValueObject
{
    internal PhoneNumber(string number)
    {
        this.Validate(number);

        this.Number = number;
    }

    public string Number { get; }

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Number;

    public static implicit operator PhoneNumber(string phoneNumber) => new (phoneNumber);

    private void Validate(string phoneNumber)
    {
        Guard.ForStringLength<InvalidPhoneNumberException>(
            phoneNumber,
            MIN_PHONE_NUMBER_LENGTH,
            MAX_PHONE_NUMBER_LENGTH,
            nameof(PhoneNumber));

        Guard.ValidateRegex(
            phoneNumber, 
            PHONE_NUMBER_REGULAR_EXPRESSION,
            "Phone number must start with a '+' and contain only digits afterwards.");
    }
}
