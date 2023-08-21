using IddyBiddyUrl.Domain;
using MongoDB.Driver;
using System.Reflection.Metadata.Ecma335;

namespace IddyBiddyUrl.Logic
{
    public class MappingService
    {
        private IMongoCollection<Mapping> _mappings;

        public MappingService(IMongoCollection<Mapping> mappings)
        {
            _mappings = mappings;   
        }

        public async Task<Mapping?> Get(string shortLink)
        {
            var cursor = await _mappings.FindAsync(m => m.ShortLink == shortLink && m.IsActive == true);
            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<Result<Mapping, ValidationException>> Create(string url, string shortLink)
        {
            var client = new HttpClient();

            try
            {
                var response = await client.GetAsync(url);
                if(!response.IsSuccessStatusCode)
                {
                    return new UrlNotValidException();
                }
            }
            catch(InvalidOperationException)
            {
                return new UrlNotValidException();
            }
            catch (HttpRequestException)
            {
                return new UrlNotValidException();
            }
            finally
            {
                client.Dispose();
            }

            try
            {
                var mapping = new Mapping { Url = url, ShortLink = shortLink, IsActive = true };
                await _mappings.InsertOneAsync(mapping);
                return mapping;

            }
            catch(MongoWriteException)
            {
                return new ShortLinkNotAvailableException();
            }
        }
    }
}
