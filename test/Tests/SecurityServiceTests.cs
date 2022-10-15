using App.Services.Security;
using App.Services.Security.Strategies;
using FluentAssertions;

namespace Tests
{
    public class SecurityServiceTests
    {
        [Fact]
        public void Given_Rsa_Certificate_Then_Should_Generate_Valid_Jwt()
        {
            // arrange
            var parameters = new SecurityParameters
            {
                Certificate = @"./Files/RSA.pfx",
                Password = "4-tests",
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

        [Fact(Skip = "https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1302")]
        public void Given_Ecc_Certificate_Then_Should_Generate_Valid_Jwt()
        {
            // arrange
            var parameters = new SecurityParameters
            {
                Certificate = @"./Files/ECC.pfx",
                Password = "4-tests",
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
