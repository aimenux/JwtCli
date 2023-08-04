using System.Text.RegularExpressions;
using App.Extensions;
using FluentAssertions;

namespace Tests.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("", "")]
    [InlineData(null, null)]
    [InlineData("abc", "abc")]
    [InlineData("abc", "Abc")]
    [InlineData("abc", "aBc")]
    [InlineData("abc", "abC")]
    [InlineData("abc", "ABC")]
    public void Should_Be_Equals(string left, string right)
    {
        // arrange
        // act
        var areEquals = left.IgnoreEquals(right);

        // assert
        areEquals.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("", null)]
    [InlineData(null, "")]
    [InlineData("abc", "âbc")]
    [InlineData("edf", "èdf")]
    [InlineData("uvw", "ùvw")]
    public void Should_Not_Be_Equals(string left, string right)
    {
        // arrange
        // act
        var areEquals = left.IgnoreEquals(right);

        // assert
        areEquals.Should().BeFalse();
    }
    
    [Fact]
    public void Should_Prettify_Json()
    {
        // arrange
        const string json = @"{""Name"":""John"",""Age"":20}";
        
        // act
        var prettyJson = json.JsonPrettify();

        // assert
        prettyJson.Should().NotBeEmpty();
        RemoveWhitespaces(prettyJson).Should().Be(json);
    }

    private static string RemoveWhitespaces(string text) => Regex.Replace(text, @"\s+", "");
}