using InfotrackTest.Interfaces;
using OpenQA.Selenium;

namespace InfotrackTest.Google
{
    public class GoogleSearchNavigator : ISearchNavigator
    {
        private const string GOOGLE_SEARCH_URL = "https://www.google.co.uk/?num=100";
        private readonly IWebDriver _webDriver;

        public GoogleSearchNavigator(IWebDriver webDriver)
        {
            _webDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
        }

        public async Task NavigateToSearchResults(string query)
        {
            // Navigate to the Google homepage
            _webDriver.Navigate().GoToUrl(GOOGLE_SEARCH_URL);

            // Find the search input box and type the query
            var searchBox = _webDriver.FindElement(By.Name("q")); // The search box has the name attribute "q"
            searchBox.Clear();
            searchBox.SendKeys(query);

            // Submit the search form
            searchBox.SendKeys(Keys.Enter);

            // Wait for the results page to load
            await WaitForResultsToLoad();
        }

        public string GetPageSource()
        {
            return _webDriver.PageSource;
        }

        private async Task WaitForResultsToLoad()
        {
            var timeout = TimeSpan.FromSeconds(10);
            var pollingInterval = TimeSpan.FromMilliseconds(500);
            var endTime = DateTime.UtcNow + timeout;

            while (DateTime.UtcNow < endTime)
            {
                try
                {
                    // Check if the search results container is present
                    if (_webDriver.FindElements(By.Id("search"))?.Count > 0)
                    {
                        return; // Results are loaded
                    }
                }
                catch
                {
                    // Ignore exceptions during polling
                }

                await Task.Delay(pollingInterval);
            }

            throw new TimeoutException("Search results did not load within the timeout period.");
        }
    }
}
