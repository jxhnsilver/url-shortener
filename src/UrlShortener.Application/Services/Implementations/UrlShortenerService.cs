using Microsoft.Extensions.Options;
using UrlShortener.Application.Contracts;
using UrlShortener.Application.Services.Interfaces;
using UrlShortener.Application.Settings;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Interfaces;

namespace UrlShortener.Application.Services.Implementations
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private const int MaxAttempts = 5;

        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly IShortCodeGenerator _shortCodeGenerator;
        private readonly string _baseUrl;

        public UrlShortenerService(
            IShortUrlRepository shortUrlRepository, 
            IShortCodeGenerator shortCodeGenerator, 
            IOptions<UrlShortenerSettings> urlOptions
            )
        {
            _shortUrlRepository = shortUrlRepository;
            _shortCodeGenerator = shortCodeGenerator;
            _baseUrl = urlOptions.Value.BaseUrl;
        }

        public async Task<CreateShortUrlResponse> CreateShortUrl(CreateShortUrlRequest request)
        {
            // TODO: Add request validation
            var existingShortUrl = await _shortUrlRepository.GetByOriginalUrlAsync(request.OriginalUrl);
            if (existingShortUrl is not null)
            {
                var fullShortUrl = $"{_baseUrl}/{existingShortUrl.ShortCode}";
                return new CreateShortUrlResponse(fullShortUrl);
            }

            for (int attempt = 0; attempt < MaxAttempts; attempt++)
            {
                var shortCode = _shortCodeGenerator.Generate();
                if (!await _shortUrlRepository.ExistsByShortCodeAsync(shortCode))
                {
                    var shortUrl = ShortUrl.Create(request.OriginalUrl, shortCode);
                    await _shortUrlRepository.AddAsync(shortUrl);

                    var fullShortUrl = $"{_baseUrl}/{shortCode}";

                    return new CreateShortUrlResponse(fullShortUrl);
                }
            }

            throw new InvalidOperationException($"Failed to generate unique short code after {MaxAttempts} attempts.");
        }

        public async Task<GetOriginalUrlResponse> GetOriginalUrl(GetOriginalUrlRequest request)
        {
            // TODO: Add request validation
            var shortUrl = await _shortUrlRepository.GetByShortCodeAsync(request.Code);
            return new GetOriginalUrlResponse(shortUrl?.OriginalUrl);
        }
    }
}
