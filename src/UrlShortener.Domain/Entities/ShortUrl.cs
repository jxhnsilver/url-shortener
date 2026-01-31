namespace UrlShortener.Domain.Entities
{
    public sealed class ShortUrl
    {
        public Guid Id { get; init; }
        public string OriginalUrl { get; init; } = null!;
        public string ShortCode { get; init; } = null!;
        private ShortUrl(Guid id, string originalUrl, string shortCode)
        {
            Id = id;
            OriginalUrl = originalUrl;
            ShortCode = shortCode;
        }
        private ShortUrl() { }

        public static ShortUrl Create(string originalUrl, string shortCode)
        {
            if (string.IsNullOrWhiteSpace(originalUrl))
                throw new ArgumentException("Original URL cannot be null or empty.", nameof(originalUrl));
            if (string.IsNullOrWhiteSpace(shortCode))
                throw new ArgumentException("Short code cannot be null or empty.", nameof(shortCode));

            return new ShortUrl(Guid.NewGuid(), originalUrl, shortCode);
        }
    }
}
