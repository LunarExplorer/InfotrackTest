namespace InfotrackTest.Interfaces
{
    public interface IHtmlFetcher
    {
        Task<string> FetchHtmlAsync(string[] keywords);
    }
}
