using InfotrackTest;
using NSubstitute;
using Xunit;

public class GoogleSEOStatsProviderTests
{
    [Fact]
    public async Task GetSeoStats_ShouldReturnCorrectPositions()
    {
        // Arrange
        var mockHtmlFetcher = Substitute.For<IHtmlFetcher>();
        string testHtml = @"
            <html>
                <body>
                    <a href='https://www.infotrack.co.uk'><h3>Result 1</h3></a>
                    <a href='https://www.example.com'><h3>Result 2</h3></a>
                    <a href='https://www.infotrack.co.uk'><h3>Result 3</h3></a>
                </body>
            </html>";
        mockHtmlFetcher.FetchHtmlAsync(Arg.Any<string[]>()).Returns(Task.FromResult(testHtml));

        var provider = new GoogleSEOStatsProvider(mockHtmlFetcher);

        // Act
        var result = await provider.GetSeoStats(new[] { "land", "registry", "search" }, "infotrack.co.uk");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(new[] { 1, 3 }, result.Positions);
    }
}
