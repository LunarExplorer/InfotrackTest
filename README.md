# SEO Position Checker - Test Context

This simple web application was created as part of a technical assessment. During the test, I encountered an unexpected requirement to retrieve search engine result positions, which inherently led to exploring screen scraping techniques against Google.

I want to explicitly state that programmatically scraping Google search results directly for production use is against their terms of service and can lead to IP blocking or other penalties. I understand this was likely a modeling exercise to assess problem-solving skills.

In a real-world production scenario, a legitimate approach to gather SEO data would involve utilizing official APIs provided by search engines (if available and suitable for the use case) or employing reputable third-party SEO tools that adhere to ethical data collection practices.

During the test, I explored various techniques, and decided to use Selenium to drive a browser in a "headed" mode to observe potential CAPTCHA challenges and Google's defenses against automated scraping. However, as anticipated, Google's systems are highly effective at detecting and preventing such automated access.
initially I tried simple URI based query but then I observed a that if I use selenium to write kyewords into Google homepage HTML elements it seems that captcha is a bit less agressive. So I left it on that.

Given the time constraints (initially stated as 3 hours, but realistically closer to 4.5 hours spent), I prioritized demonstrating my abilities in:

* **Object-Oriented Programming (OOP):** The underlying code structure (though not fully represented in this simple UI) was designed with OOP principles in mind for better organization and maintainability.
* **Testability:** While a comprehensive UI within the time limit proved challenging, the foundational code was structured to facilitate unit testing and integration testing.

Unfortunately, due to the significant time spent investigating the complexities of bypassing Google's anti-scraping measures, I ran out of time to build a more elaborate user interface within the given timeframe. I believe continuing significantly beyond the extended time would be unethical.


The api documentation:
```
curl --location 'http://localhost:5063/api/seostats/positions/?null=null&keywords=land%2Cregistry%2Csearch&subjectUrl=infotrack.co.uk'
```
response http 200
```json
{
    "positions": [18,25]
}
```
The API is designed to accept a list of keywords and a subject URL, returning the search engine result positions for the specified keywords. This is simplest cacheable endpoint I can think of. Test say string of ints I return proper int array. UI could cast it to whatever it likes.  

while running a project you might need to breakpoint on string htmlContent = await _htmlFetcher.FetchHtmlAsync(keywords); and observe the captcha challenge

```        public async Task<SeoStats> GetSeoStats(string[] keywords, string subjectUrl)
        {
            // Fetch the HTML content using the injected IHtmlFetcher
            string htmlContent = await _htmlFetcher.FetchHtmlAsync(keywords);

            // Parse the HTML to find the positions of the subject URL
            var positions = FindUrlPositions(htmlContent, subjectUrl);

            return new SeoStats
            {
                Positions = positions
            };
        }
```
Ocassionally I have observed different consent screens come up. Accept those then let program to run. 