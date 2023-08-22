namespace UrlShortenerService.Validation
{
    public class CouldNotGenerateShortLinkException : ValidationException
    {
        public CouldNotGenerateShortLinkException() : base("Could not generate short link")
        {
        }
    }
}
