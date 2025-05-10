using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace InfotrackTest
{
    public class SeleniumHtmlFetcher : IHtmlFetcher
    {
        private readonly IWebDriver _webDriver;

        public SeleniumHtmlFetcher(IWebDriver webDriver)
        {
            _webDriver = webDriver ?? throw new ArgumentNullException(nameof(webDriver));
        }
        /// <summary>
        /// Maybe there is a benefit to write into search input text box rather than using query string
        /// as Goolgle clearly states that it is not like their search results being scraped
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public async Task<string> FetchHtmlAsync(string[] keywords)
        {
            string query = string.Join("+", keywords);
            string searchUrl = $"https://www.google.co.uk/search?num=100&q={query}";

            _webDriver.Navigate().GoToUrl(searchUrl);

            // Check if CAPTCHA is present and wait for it to be solved
            if (_webDriver.PageSource.Contains("detected unusual traffic"))
            {
                Console.WriteLine("CAPTCHA detected. Waiting for it to be solved...");
                await WaitForCaptchaToBeSolved(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(5));
            }

            return _webDriver.PageSource;
        }

        private async Task WaitForCaptchaToBeSolved(TimeSpan timeout, TimeSpan pollingInterval)
        {
            var endTime = DateTime.UtcNow + timeout;

            while (DateTime.UtcNow < endTime)
            {
                if (!_webDriver.PageSource.Contains("detected unusual traffic"))
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
