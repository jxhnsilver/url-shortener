using System.Text.RegularExpressions;
using UrlShortener.Application.Services.Implementations;

namespace UrlShortener.Application.Tests
{
    public class RandomShortCodeGeneratorTests
    {
        [Theory]
        [InlineData(6)]
        [InlineData(9)]
        [InlineData(10)]
        public void Generate_WithValidLength_ReturnsStringWithCorrectLength(int length)
        {
            var generator = new RandomShortCodeGenerator(length);
            var result = generator.Generate();

            Assert.Equal(length, result.Length);
        }

        [Theory]
        [InlineData(100, 10)]
        public void Generate_ReturnsStringContainingAllowedChars(int iterations, int codeLength)
        {
            var generator = new RandomShortCodeGenerator(codeLength);
            var pattern = $"^[{Regex.Escape(RandomShortCodeGenerator.Alphabet)}]+$";

            for (int i = 0; i < iterations; i++)
            {
                var result = generator.Generate();
                Assert.Matches(pattern, result);
            }
        }

        [Theory]
        [InlineData(5)]
        [InlineData(11)]
        public void Constructor_WhenCodeLengthOutOfRange_ThrowsArgumentOutOfRangeException(int invalidLength)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() 
                => new RandomShortCodeGenerator(invalidLength));
            var message = "Short code length must be between 6 and 10";


            Assert.Contains(message, exception.Message);
            Assert.Equal("codeLength", exception.ParamName);
        }
    }
}
