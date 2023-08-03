using App.Services.Security;
using FluentAssertions;

namespace Tests.Services
{
    public class CertificateParametersTests
    {
        [Fact]
        public void Given_Valid_File_Then_Should_Build_Certificate_Parameters()
        {
            // arrange
            const string parametersFile = @"./Files/CertificateParameters.json";

            // act
            var parameters = SecurityParameters.BuildFromFile(parametersFile);

            // assert
            parameters.Should().NotBeNull();
            parameters.Certificate.Should().NotBeNullOrWhiteSpace();
            parameters.Password.Should().NotBeNullOrWhiteSpace();
        }
    }
}
