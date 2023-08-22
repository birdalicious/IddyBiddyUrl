namespace IddyBiddyUrl.Validation
{
    public class UrlNotValidException : ValidationException
    {
        public UrlNotValidException() : base("Url is not valid link")
        {

        }
    }
}
