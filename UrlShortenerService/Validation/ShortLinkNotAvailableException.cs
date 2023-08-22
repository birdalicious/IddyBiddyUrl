namespace UrlShortenerService.Validation
{
    public class ShortLinkNotAvailableException : ValidationException
    {
        public ShortLinkNotAvailableException() : base("Short Link is not available") { }
        public ShortLinkNotAvailableException(string message) : base(message) { }
    }
}
