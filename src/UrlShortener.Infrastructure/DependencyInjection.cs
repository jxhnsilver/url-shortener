using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Services.Implementations;
using UrlShortener.Application.Services.Interfaces;
using UrlShortener.Domain.Interfaces;
using UrlShortener.Infrastructure.Context;
using UrlShortener.Infrastructure.Repositories;

namespace UrlShortener.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IShortUrlRepository, ShortUrlRepository>();

            return services;
        }
    }
}
