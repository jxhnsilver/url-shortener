using UrlShortener.Domain.Entities;

namespace UrlShortener.Domain.Tests
{
    public class ShortUrlTests
    {
        [Fact]
        public void Create_WithValidParameters_ReturnsShortUrlWithNewGuidId()
        {
            var originalUrl = "https://test-url.com";
            var shortCode = "Test123";

            var result1 = ShortUrl.Create(originalUrl, shortCode);
            var result2 = ShortUrl.Create(originalUrl, shortCode);

            Assert.NotNull(result1);
            Assert.Equal(originalUrl, result1.OriginalUrl);
            Assert.Equal(shortCode, result1.ShortCode);
            Assert.NotEqual(result1.Id, result2.Id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithNullEmptyOrWhiteSpaceOriginalUrl_ThrowsArgumentException(string invalidOriginalUrl)
        {
            var shortCode = "Test123";
            var expectedMessage = "Original URL cannot be null or empty.";

            var exception = Assert.Throws<ArgumentException> (() 
                => ShortUrl.Create(invalidOriginalUrl, shortCode));
            
            Assert.Contains(expectedMessage, exception.Message);
            Assert.Equal("originalUrl", exception.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithNullEmptyOrWhiteSpaceShortCode_ThrowsArgumentException(string invalidCode)
        {
            var originalUrl = "https://test-url.com";
            var message = "Short code cannot be null or empty.";

            var exception = Assert.Throws<ArgumentException>(()
                => ShortUrl.Create(originalUrl, invalidCode));

            Assert.Contains(message, exception.Message);
            Assert.Equal("shortCode", exception.ParamName);
        }
    }
}
