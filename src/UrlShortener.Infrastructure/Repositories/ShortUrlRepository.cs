using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Interfaces;
using UrlShortener.Infrastructure.Context;

namespace UrlShortener.Infrastructure.Repositories
{
    public class ShortUrlRepository : IShortUrlRepository
    {
        private readonly ApplicationDbContext _context;
        public ShortUrlRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(ShortUrl shortUrl)
        {
            await _context.AddAsync(shortUrl);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByShortCodeAsync(string shortCode)
        {
            return await _context.ShortUrls.AnyAsync(s => s.ShortCode == shortCode);
        }

        public async Task<ShortUrl?> GetByOriginalUrlAsync(string originalUrl)
        {
            return await _context.ShortUrls.FirstOrDefaultAsync(s => s.OriginalUrl == originalUrl);
        }

        public async Task<ShortUrl?> GetByShortCodeAsync(string shortCode)
        {
            return await _context.ShortUrls.FirstOrDefaultAsync(s => s.ShortCode == shortCode);
        }
    }
}
