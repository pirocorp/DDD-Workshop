namespace CarRentalSystem.Fakes.Infrastructure.Identity;

using CarRentalSystem.Infrastructure.Identity;

using FakeItEasy;

public class JwtTokenGeneratorFakes
{
    public const string VALID_TOKEN = "ValidToken";

    public static IJwtTokenGenerator FakeJwtTokenGenerator
    {
        get
        {
            var jwtTokenGenerator = A.Fake<IJwtTokenGenerator>();

            A
                .CallTo(() => jwtTokenGenerator.GenerateToken(A<User>.Ignored))
                .Returns(VALID_TOKEN);

            return jwtTokenGenerator;
        }
    }
}
