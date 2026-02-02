using UrlShortener.Application.Services.Interfaces;

namespace UrlShortener.Application.Services.Implementations
{
    public class RandomShortCodeGenerator : IShortCodeGenerator
    {
        public const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int MinCodeLength = 6;
        private const int MaxCodeLength = 10;

        private readonly Random _random = Random.Shared;
        private readonly int _codeLength;

        public RandomShortCodeGenerator(int codeLength)
        {
            if (codeLength < MinCodeLength || codeLength > MaxCodeLength)
                throw new ArgumentOutOfRangeException(
                    nameof(codeLength),
                    $"Short code length must be between {MinCodeLength} and {MaxCodeLength}."
                    );

            _codeLength = codeLength;
        }

        public string Generate()
        {
            var codeChars = new char[_codeLength];
            for (int i = 0; i < _codeLength; i++)
            {
                int randomIndex = _random.Next(Alphabet.Length);
                codeChars[i] = Alphabet[randomIndex];
            }

            var shortCode = new string(codeChars);
            return shortCode;
        }
    }
}
