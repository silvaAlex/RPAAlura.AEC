namespace RPAAlura.AEC.Domain.Entities
{
    public class UrlAlura
    {
        public List<string> Urls { get; private set; } = new List<string>();

        public void AddUrl(string url)
        {
            Urls.Add(url);
        }
    }
}
