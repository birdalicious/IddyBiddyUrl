namespace IddyBiddyUrl.Logic
{
    public class UrlNotValidException : ValidationException
    {
        public UrlNotValidException() : base("Url is not valid link")
        {

        }
    }
}
