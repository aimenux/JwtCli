using App.Services.Security;
using App.Services.Security.Strategies;
using FluentAssertions;

namespace Tests
{
    public class EccCertificateStrategyTests
    {
        [Fact]
        public void Given_Ecc_Certificate_Then_Should_Match_Ecc_Strategy()
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

            var strategy = new EccCertificateStrategy();

            // act
            var isMatching = strategy.IsMatching(parameters);

            // assert
            isMatching.Should().BeTrue();
        }

        [Fact]
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

            var strategy = new EccCertificateStrategy();

            // act
            var jwt = strategy.CreateJwtToken(parameters);
            var isValid = strategy.VerifyJwtToken(jwt, parameters);

            // assert
            jwt.Should().NotBeNullOrWhiteSpace();
            isValid.Should().BeTrue();
        }
    }
}