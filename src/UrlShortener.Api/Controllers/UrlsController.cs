using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Contracts;
using UrlShortener.Application.Services.Interfaces;

namespace UrlShortener.Api.Controllers
{
    [Route("api/urls")]
    [ApiController]
    public class UrlsController : ControllerBase
    {
        private readonly IUrlShortenerService _urlShortenerService;
        public UrlsController(IUrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }

        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] CreateShortUrlRequest request)
        {
            var response = await _urlShortenerService.CreateShortUrl(request);
            return Ok(response);
        }
    }
}
