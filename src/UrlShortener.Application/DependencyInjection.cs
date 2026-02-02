using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UrlShortener.Application.Services.Implementations;
using UrlShortener.Application.Services.Interfaces;
using UrlShortener.Application.Settings;

namespace UrlShortener.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<UrlShortenerSettings>(configuration.GetSection("UrlShortenerSettings"));

            services.AddScoped<IUrlShortenerService, UrlShortenerService>();

            services.AddSingleton<IShortCodeGenerator>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<UrlShortenerSettings>>().Value;
                return new RandomShortCodeGenerator(settings.CodeLength);
            });

            return services;
        }
    }
}
