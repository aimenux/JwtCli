using App.Services.Security;
using App.Services.Security.Strategies;
using FluentAssertions;

namespace Tests
{
    public class RsaCertificateStrategyTests
    {
        [Fact]
        public void Given_Rsa_Certificate_Then_Should_Match_Rsa_Strategy()
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

            var strategy = new RsaCertificateStrategy();

            // act
            var isMatching = strategy.IsMatching(parameters);

            // assert
            isMatching.Should().BeTrue();
        }

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

            var strategy = new RsaCertificateStrategy();

            // act
            var jwt = strategy.CreateJwtToken(parameters);
            var isValid = strategy.VerifyJwtToken(jwt, parameters);

            // assert
            jwt.Should().NotBeNullOrWhiteSpace();
            isValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("jwt", "", true)]
        [InlineData("jwt", null, true)]
        [InlineData("jwt", "jwt", true)]
        [InlineData("jwt", "jwt+", false)]
        [InlineData("jwt", "+jwt", false)]
        [InlineData("jwt", "toto", false)]
        public void Given_Rsa_Certificate_Then_Should_Validate_Jwt(string firstTokenType, string secondTokenType, bool expectedIsValid)
        {
            // arrange
            var generateParameters = new SecurityParameters
            {
                Certificate = @"./Files/RSA.pfx",
                Password = "4-tests",
                Audience = Guid.NewGuid().ToString(),
                Issuer = Guid.NewGuid().ToString(),
                Kid = Guid.NewGuid().ToString(),
                ExpireInMinutes = 5,
                TokenType = firstTokenType
            };

            var validateParameters = new SecurityParameters(generateParameters)
            {
                TokenType = secondTokenType
            };

            var strategy = new RsaCertificateStrategy();

            // act
            var jwt = strategy.CreateJwtToken(generateParameters);
            var isValid = strategy.VerifyJwtToken(jwt, validateParameters);

            // assert
            jwt.Should().NotBeNullOrWhiteSpace();
            isValid.Should().Be(expectedIsValid);
        }
    }
}