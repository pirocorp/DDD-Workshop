namespace CarRentalSystem.Application;

public class ApplicationSettings
{
    public ApplicationSettings()
    {
        this.Secret = string.Empty;
    }

    public string Secret { get; private set; }
}
