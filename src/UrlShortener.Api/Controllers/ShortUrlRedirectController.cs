using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Contracts;
using UrlShortener.Application.Services.Interfaces;

namespace UrlShortener.Api.Controllers
{
    public class ShortUrlRedirectController : Controller
    {
        private readonly IUrlShortenerService _urlShortenerService;

        public ShortUrlRedirectController(IUrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }

        [HttpGet("/{code}")]
        public async Task<IActionResult> RedirectToOriginal(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return NotFound();

            var response = await _urlShortenerService.GetOriginalUrl(new GetOriginalUrlRequest(code));

            if (response.OriginalUrl is null)
                return NotFound();

            return Redirect(response.OriginalUrl);
        }
    }
}
