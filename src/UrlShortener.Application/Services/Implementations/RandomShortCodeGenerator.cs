using UrlShortener.Application.Services.Interfaces;

namespace UrlShortener.Application.Services.Implementations
{
    public class RandomShortCodeGenerator : IShortCodeGenerator
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly Random _random = Random.Shared;

        public string Generate(int shortCodeLength)
        {
            var codeChars = new char[shortCodeLength];
            for (int i = 0; i < shortCodeLength; i++)
            {
                int randomIndex = _random.Next(Alphabet.Length - 1);
                codeChars[i] = Alphabet[randomIndex];
            }

            var shortCode = new string(codeChars);
            return shortCode;
        }
    }
}
