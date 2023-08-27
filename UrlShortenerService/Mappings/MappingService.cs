using MongoDB.Driver;
using UrlShortenerService.Validation;

namespace UrlShortenerService.Mappings
{
    public class MappingService
    {
        private IMongoCollection<Mapping> _mappings;
        private HttpClient _httpClient;

        public MappingService(IMongoCollection<Mapping> mappings, HttpClient httpClient)
        {
            _mappings = mappings;
            _httpClient = httpClient;
        }

        public async Task<Mapping?> GetAsync(string shortLink)
        {
            var cursor = await _mappings.FindAsync(m => m.ShortLink == shortLink && m.IsActive == true);
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<Result<Mapping, ValidationException>> Create(string url)
        {
            string shortLink;
            try
            {
                shortLink = await GetUnquieShortLinkAsync();
            }
            catch (CouldNotGenerateShortLinkException ex)
            {
                return ex;
            }

            return await Create(url, shortLink);
        }

        public async Task<Result<Mapping, ValidationException>> Create(string url, string shortLink)
        {
            if (string.IsNullOrWhiteSpace(shortLink))
            {
                return new ShortLinkNotAvailableException("Must provide non-blank short link");
            }

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return new UrlNotValidException();
                }
            }
            catch (InvalidOperationException)
            {
                return new UrlNotValidException();
            }
            catch (HttpRequestException)
            {
                return new UrlNotValidException();
            }

            try
            {
                var mapping = new Mapping { Url = url, ShortLink = shortLink, IsActive = true };
                await _mappings.InsertOneAsync(mapping);
                return mapping;

            }
            catch (MongoWriteException)
            {
                return new ShortLinkNotAvailableException();
            }
        }

        private async Task<string> GetUnquieShortLinkAsync()
        {
            const int shortLinkLength = 5;
            Mapping? mapping = null;
            string shortLinkCandiate;

            var tries = 0;
            do
            {
                shortLinkCandiate = RandomStringGenerator.GetString(shortLinkLength + tries);
                var cursor = await _mappings.FindAsync(m => m.ShortLink == shortLinkCandiate);
                mapping = await cursor.FirstOrDefaultAsync();

                tries++;
            }
            while (mapping is not null && tries < 3);

            if (mapping is not null)
            {
                throw new CouldNotGenerateShortLinkException();
            }

            return shortLinkCandiate;
        }
    }
}
