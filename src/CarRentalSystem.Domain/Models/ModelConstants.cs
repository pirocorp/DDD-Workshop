namespace CarRentalSystem.Domain.Models;

public class ModelConstants
{
    public class Common
    {
        public const int MIN_NAME_LENGTH = 2;
        public const int MAX_NAME_LENGTH = 20;
        public const int MIN_EMAIL_LENGTH = 3;
        public const int MAX_EMAIL_LENGTH = 50;
        public const int MAX_URL_LENGTH = 2048;
        public const int ZERO = 0;
    }

    public class Category
    {
        public const int MIN_DESCRIPTION_LENGTH = 20;
        public const int MAX_DESCRIPTION_LENGTH = 1000;
    }

    public class Options
    {
        public const int MIN_NUMBER_OF_SEATS = 2;
        public const int MAX_NUMBER_OF_SEATS = 50;
    }

    public class PhoneNumber
    {
        public const int MIN_PHONE_NUMBER_LENGTH = 5;
        public const int MAX_PHONE_NUMBER_LENGTH = 20;
        public const string PHONE_NUMBER_REGULAR_EXPRESSION = @"\+[0-9]*";
    }

    public class CarAd
    {
        public const int MIN_MODEL_LENGTH = 2;
        public const int MAX_MODEL_LENGTH = 20;
    }
}
