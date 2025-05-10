using InfotrackTest.Interfaces;
using System;

namespace InfotrackTest.Google
{
    public class GoogleSeleniumHtmlFetcher : IHtmlFetcher
    {
        private readonly ISearchNavigator _navigator;

        public GoogleSeleniumHtmlFetcher(ISearchNavigator navigator)
        {
            _navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
        }

        public async Task<string> FetchHtmlAsync(string[] keywords)
        {
            string query = string.Join(" ", keywords); // Use space-separated keywords for the search box

            // Navigate to search results
            await _navigator.NavigateToSearchResults(query);

            // Check if CAPTCHA is present and wait for it to be solved
            if (_navigator.GetPageSource().Contains("detected unusual traffic"))
            {
                Console.WriteLine("CAPTCHA detected. Waiting for it to be solved...");
                await WaitForCaptchaToBeSolved(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(5));
            }

            return _navigator.GetPageSource();
        }

        private async Task WaitForCaptchaToBeSolved(TimeSpan timeout, TimeSpan pollingInterval)
        {
            var endTime = DateTime.UtcNow + timeout;

            while (DateTime.UtcNow < endTime)
            {
                if (!_navigator.GetPageSource().Contains("detected unusual traffic"))
                {
                    Console.WriteLine("CAPTCHA solved or not present.");
                    return;
                }

                await Task.Delay(pollingInterval);
            }

            throw new TimeoutException("CAPTCHA was not solved within the timeout period.");
        }
    }
}
