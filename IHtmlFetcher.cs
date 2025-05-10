namespace InfotrackTest
{
    public interface IHtmlFetcher
    {
        Task<string> FetchHtmlAsync(string[] keywords);
    }
}
