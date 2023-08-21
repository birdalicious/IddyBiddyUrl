namespace IddyBiddyUrl.Models
{
    public class CreateShortUrl
    {
        public string Url { get; set; }
        public string ShortLink { get; set; }   
        public bool GenerateShortLink { get; set; }
    }
}
