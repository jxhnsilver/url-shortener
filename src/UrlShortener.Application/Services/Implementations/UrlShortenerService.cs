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
        private const int ShortCodeLength = 7;

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
            var existingShortUrl = await _shortUrlRepository.GetByOriginalUrlAsync(request.OriginalUrl);
            if (existingShortUrl is not null)
            {
                var fullShortUrl = $"{_baseUrl}/{existingShortUrl.ShortCode}";
                return new CreateShortUrlResponse(fullShortUrl);
            }

            // TODO: Add max attempts limit for short code generation
            while (true) 
            {
                var shortCode = _shortCodeGenerator.Generate(ShortCodeLength);
                if (!await _shortUrlRepository.ExistsByShortCodeAsync(shortCode))
                {
                    var shortUrl = ShortUrl.Create(request.OriginalUrl, shortCode);
                    await _shortUrlRepository.AddAsync(shortUrl);

                    var fullShortUrl = $"{_baseUrl}/{shortCode}";

                    return new CreateShortUrlResponse(fullShortUrl);
                }
            }
        }

        public async Task<GetOriginalUrlResponse> GetOriginalUrl(GetOriginalUrlRequest request)
        {
            var shortUrl = await _shortUrlRepository.GetByShortCodeAsync(request.Code);
            return new GetOriginalUrlResponse(shortUrl?.OriginalUrl);
        }
    }
}
