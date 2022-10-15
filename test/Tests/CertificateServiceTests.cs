using App.Services.Security;
using App.Services.Security.Strategies;
using FluentAssertions;

namespace Tests
{
    public class CertificateServiceTests
    {
        [Theory]
        [InlineData(@"./Files/RSA.pfx", "4-tests")]
        [InlineData(@"./Files/ECC.pfx", "4-tests")]
        public void Given_Valid_Certificate_Then_Should_Generate_Valid_Jwt(string certificate, string password)
        {
            // arrange
            var parameters = new SecurityParameters
            {
                Certificate = certificate,
                Password = password,
                Audience = Guid.NewGuid().ToString(),
                Issuer = Guid.NewGuid().ToString(),
                Kid = Guid.NewGuid().ToString(),
                ExpireInMinutes = 5,
                TokenType = "jwt"
            };

            var strategies = new List<ICertificateStrategy>
            {
                new RsaCertificateStrategy(),
                new EccCertificateStrategy()
            };
            var service = new SecurityService(strategies);

            // act
            var jwt = service.GenerateJwtToken(parameters);
            var isValid = service.ValidateJwtToken(jwt, parameters);

            // assert
            jwt.Should().NotBeNullOrWhiteSpace();
            isValid.Should().BeTrue();
        }
    }
}
