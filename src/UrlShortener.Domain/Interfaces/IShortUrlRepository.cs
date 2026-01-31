using UrlShortener.Domain.Entities;

namespace UrlShortener.Domain.Interfaces
{
    public interface IShortUrlRepository
    {
        Task AddAsync(ShortUrl shortUrl);
        Task<ShortUrl?> GetByOriginalUrlAsync(string originalUrl);
        Task<ShortUrl?> GetByShortCodeAsync(string shortCode);
        Task<bool> ExistsByShortCodeAsync(string  shortCode);
    }
}
