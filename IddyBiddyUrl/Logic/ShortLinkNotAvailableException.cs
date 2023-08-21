namespace IddyBiddyUrl.Logic
{
    public class ShortLinkNotAvailableException : ValidationException
    {
        public ShortLinkNotAvailableException() : base("Short Link is not available") { }
    }
}
