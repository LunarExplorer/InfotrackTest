using HtmlAgilityPack;
using OpenQA.Selenium;

namespace InfotrackTest
{
    public class GoogleSEOStatsProvider : ISeoStatsProvider
    {
        private readonly IHtmlFetcher _htmlFetcher;

        public GoogleSEOStatsProvider(IHtmlFetcher htmlFetcher)
        {
            _htmlFetcher = htmlFetcher ?? throw new ArgumentNullException(nameof(htmlFetcher));
        }

        public async Task<SeoStats> GetSeoStats(string[] keywords, string subjectUrl)
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

        private int[] FindUrlPositions(string htmlContent, string subjectUrl)
        {
            // Load the HTML content into HtmlAgilityPack
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            // Find all <a> elements with href attributes
            var linkNodes = htmlDoc.DocumentNode.SelectNodes("//a[@href]");

            var positions = new List<int>();
            int position = 0;

            if (linkNodes != null)
            {
                foreach (var linkNode in linkNodes)
                {
                    string href = linkNode.GetAttributeValue("href", string.Empty);

                    // Check if the link is part of the search results (e.g., contains an <h3> child)
                    if (linkNode?.FirstChild?.Name?.ToLowerInvariant() == "h3")
                    {
                        position++;
                    }

                    // Check if the href contains the subject URL
                    if (!string.IsNullOrEmpty(href) && href.Contains(subjectUrl, StringComparison.OrdinalIgnoreCase))
                    {
                        positions.Add(position);
                    }
                }
            }

            return positions.ToArray();
        }
    }
}
