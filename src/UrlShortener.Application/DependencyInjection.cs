using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddSingleton<IShortCodeGenerator, RandomShortCodeGenerator>();

            return services;
        }
    }
}
