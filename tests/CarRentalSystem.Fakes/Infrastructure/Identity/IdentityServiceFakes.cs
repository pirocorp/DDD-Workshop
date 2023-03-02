namespace CarRentalSystem.Fakes.Infrastructure.Identity;

using CarRentalSystem.Infrastructure.Identity;

using FakeItEasy;

using Microsoft.AspNetCore.Identity;

public class IdentityFakes
{
    public const string TEST_EMAIL = "test@test.com";
    public const string VALID_PASSWORD = "TestPass";

    public static UserManager<User> FakeUserManager
    {
        get
        {
            var userManager = A.Fake<UserManager<User>>();

            A
                .CallTo(() => userManager.FindByEmailAsync(TEST_EMAIL))
                .Returns(new User(TEST_EMAIL) { Id = "test" });

            A
                .CallTo(() => userManager.CheckPasswordAsync(A<User>.That.Matches(u => u.Email == TEST_EMAIL), VALID_PASSWORD))
                .Returns(true);

            return userManager;
        }
    }
}

