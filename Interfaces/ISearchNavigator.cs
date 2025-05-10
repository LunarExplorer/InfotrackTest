namespace InfotrackTest.Interfaces
{
    public interface ISearchNavigator
    {
        string GetPageSource();
        Task NavigateToSearchResults(string query);
    }
}