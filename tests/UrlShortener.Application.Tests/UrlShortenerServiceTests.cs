using Microsoft.Extensions.Options;
using Moq;
using UrlShortener.Application.Contracts;
using UrlShortener.Application.Services.Implementations;
using UrlShortener.Application.Services.Interfaces;
using UrlShortener.Application.Settings;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Interfaces;

namespace UrlShortener.Application.Tests
{
    public class UrlShortenerServiceTests
    {
        private readonly Mock<IShortUrlRepository> _shortUrlRepositoryMock;
        private readonly Mock<IShortCodeGenerator> _shortCodeGeneratorMock;
        private readonly UrlShortenerService _urlShortenerService;

        private const string TestBaseUrl = "https://testurl.com";
        public UrlShortenerServiceTests()
        {
            _shortUrlRepositoryMock = new Mock<IShortUrlRepository>();
            _shortCodeGeneratorMock = new Mock<IShortCodeGenerator>();

            var fakeOptions = Options.Create(
                new UrlShortenerSettings
                { 
                    BaseUrl = TestBaseUrl
                }
            );

            _urlShortenerService = new UrlShortenerService(
                _shortUrlRepositoryMock.Object, 
                _shortCodeGeneratorMock.Object, 
                fakeOptions
                );
        }

        [Fact]
        public async Task CreateShortUrl_WhenUrlAlreadyExists_ReturnsExistingShortUrl()
        {
            // Arrange
            var originalUrl = "https://example.com";
            var existingCode = "TestCode123";
            var request = new CreateShortUrlRequest(originalUrl);
            var expectedFullUrl = $"{TestBaseUrl}{existingCode}";

            var existingShortUrl = ShortUrl.Create(originalUrl, existingCode);
            _shortUrlRepositoryMock
                .Setup(r => r.GetByOriginalUrlAsync(request.OriginalUrl))
                .ReturnsAsync(existingShortUrl);

            // Act
            var response = await _urlShortenerService.CreateShortUrl(request);

            // Assert
            Assert.Equal($"{TestBaseUrl}/{existingCode}", response.FullShortUrl);
            _shortCodeGeneratorMock.Verify(g => g.Generate(), Times.Never);
        }

        [Fact]
        public async Task CreateShortUrl_WhenUrlDoesNotExists_GeneratesNewShortCodeAndReturnsShortUrl()
        {
            // Arrange
            var originalUrl = "https://example.com";
            var request = new CreateShortUrlRequest(originalUrl);
            var generatedCode = "TestCode123";

            _shortUrlRepositoryMock
                .Setup(r => r.GetByOriginalUrlAsync(request.OriginalUrl))
                .ReturnsAsync((ShortUrl?)null);
            _shortCodeGeneratorMock
                .Setup(r => r.Generate())
                .Returns(generatedCode);
            _shortUrlRepositoryMock
                .Setup(r => r.ExistsByShortCodeAsync(generatedCode))
                .ReturnsAsync(false);

            // Act
            var result = await _urlShortenerService.CreateShortUrl(request);

            // Assert
            Assert.Equal($"{TestBaseUrl}/{generatedCode}", result.FullShortUrl);
            _shortUrlRepositoryMock
                .Verify(r => r.AddAsync(It.Is<ShortUrl>(s => s.ShortCode == generatedCode)), Times.Once);
        }

        [Fact]
        public async Task CreateShortUrl_WhenColiisionGeneratedShortCode_RetriesAndSucceeds()
        {
            // Arrange
            var originalUrl = "https://example.com";
            var request = new CreateShortUrlRequest(originalUrl);
            var firstCode = "Taken";
            var secondCode = "Free";

            _shortUrlRepositoryMock
                .Setup(r => r.GetByOriginalUrlAsync(request.OriginalUrl))
                .ReturnsAsync((ShortUrl?)null);
            _shortCodeGeneratorMock.SetupSequence(g => g.Generate())
                .Returns(firstCode)
                .Returns(secondCode);
            _shortUrlRepositoryMock.Setup(r => r.ExistsByShortCodeAsync(firstCode))
                .ReturnsAsync(true);
            _shortUrlRepositoryMock.Setup(r => r.ExistsByShortCodeAsync(secondCode))
                .ReturnsAsync(false);

            // Act
            var result = await _urlShortenerService.CreateShortUrl(request);

            // Assert
            Assert.Equal($"{TestBaseUrl}/{secondCode}", result.FullShortUrl);
            _shortCodeGeneratorMock.Verify(g => g.Generate(), Times.Exactly(2));
        }

        [Fact]
        public async Task CreateShortUrl_WhenMaxAttemptsReached_ThrowsInvalidOperationException()
        {
            // Arrange
            var originalUrl = "https://example.com";
            var request = new CreateShortUrlRequest(originalUrl);
            var message = "Failed to generate unique short code after 5 attempts.";

            _shortUrlRepositoryMock
                .Setup(r => r.GetByOriginalUrlAsync(request.OriginalUrl))
                .ReturnsAsync((ShortUrl?)null);
            _shortUrlRepositoryMock.Setup(r => r.ExistsByShortCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(()
                => _urlShortenerService.CreateShortUrl(request));

            // Assert
            Assert.Contains(message, exception.Message);
        }

        [Fact]
        public async Task GetOriginalUrl_WhenShortCodeExists_ReturnsOriginalUrl()
        {
            // Arrange
            var originalUrl = "https://example.com";
            var code = "TestCode1";
            var shortUrl = ShortUrl.Create(originalUrl, code);

            _shortUrlRepositoryMock
                .Setup(r => r.GetByShortCodeAsync(code))
                .ReturnsAsync(shortUrl);

            var request = new GetOriginalUrlRequest(code);

            // Act
            var result = await _urlShortenerService.GetOriginalUrl(request);

            // Arrange
            Assert.Equal(originalUrl, result.OriginalUrl);
        }

        [Fact]
        public async Task GetOriginalUrl_WhenCodeNotFound_ReturnsNullOriginalUrl()
        {
            // Arrange
            var code = "TestCode";
            _shortUrlRepositoryMock
                .Setup(r => r.GetByShortCodeAsync(code))
                .ReturnsAsync((ShortUrl?)null);

            var request = new GetOriginalUrlRequest(code);

            // Act
            var result = await _urlShortenerService.GetOriginalUrl(request);

            // Assert
            Assert.Null(result.OriginalUrl);
        }
    }
}
