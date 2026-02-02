using UrlShortener.Application.Contracts;

namespace UrlShortener.Application.Services.Interfaces
{
    public interface IUrlShortenerService
    {
        Task<CreateShortUrlResponse> CreateShortUrl(CreateShortUrlRequest request);
        Task<GetOriginalUrlResponse> GetOriginalUrl(GetOriginalUrlRequest request);
    }
}
