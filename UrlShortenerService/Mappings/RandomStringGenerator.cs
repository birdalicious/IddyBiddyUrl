namespace UrlShortenerService.Mappings
{
    public static class RandomStringGenerator
    {
        public static string GetString(int length = 4)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            if (length > 24)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())[..length];
        }
    }
}
