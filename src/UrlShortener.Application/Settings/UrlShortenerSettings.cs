namespace UrlShortener.Application.Settings
{
    public class UrlShortenerSettings
    {
        public string BaseUrl { get; set; } = null!;
        public int CodeLength { get; set; }
    }
}
