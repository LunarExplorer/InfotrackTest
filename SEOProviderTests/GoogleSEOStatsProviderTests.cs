using InfotrackTest;
using InfotrackTest.Google;
using InfotrackTest.Interfaces;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using OpenQA.Selenium;
using Xunit;

public class GoogleSEOStatsProviderTests
{
    [Fact]
    public async Task GetSeoStats_ShouldReturnCorrectPositions_WithMoreThanOne_Result()
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

    [Fact]
    public async Task GetSeoStats_ShouldReturnCorrectPositions_FromHtmlFile()
    {
        // Arrange
        var mockHtmlFetcher = Substitute.For<IHtmlFetcher>();

        // Construct the relative path to the HTML file
        string filePath = Path.Combine(AppContext.BaseDirectory, "MockData", "SampleGoogleQueryResult.html");

        // Read the HTML content from the file
        string testHtml = await File.ReadAllTextAsync(filePath);

        // Configure the mock to return the file content
        mockHtmlFetcher.FetchHtmlAsync(Arg.Any<string[]>()).Returns(Task.FromResult(testHtml));

        var provider = new GoogleSEOStatsProvider(mockHtmlFetcher);

        // Act
        var result = await provider.GetSeoStats(new[] { "land", "registry", "search" }, "infotrack.co.uk");

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Positions);
        Assert.All(result.Positions, position => Assert.True(position > 0));
        Assert.Equal(20, result.Positions[0]);
    }

    [Fact]
    public async Task GetSeoStats_ShouldHandleNoResults()
    {
        // Arrange
        var mockHtmlFetcher = Substitute.For<IHtmlFetcher>();
        string testHtml = @"
            <html>
                <body>
                    <div id='search'>No results found</div>
                </body>
            </html>";
        mockHtmlFetcher.FetchHtmlAsync(Arg.Any<string[]>()).Returns(Task.FromResult(testHtml));

        var provider = new GoogleSEOStatsProvider(mockHtmlFetcher);

        // Act
        var result = await provider.GetSeoStats(new[] { "nonexistent", "query" }, "infotrack.co.uk");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Positions);
    }

    [Fact]
    public async Task GetSeoStats_ShouldHandleInvalidHtml()
    {
        // Arrange
        var mockHtmlFetcher = Substitute.For<IHtmlFetcher>();
        string testHtml = "<html><body><div>Invalid HTML structure</div></body></html>";
        mockHtmlFetcher.FetchHtmlAsync(Arg.Any<string[]>()).Returns(Task.FromResult(testHtml));

        var provider = new GoogleSEOStatsProvider(mockHtmlFetcher);

        // Act
        var result = await provider.GetSeoStats(new[] { "land", "registry", "search" }, "infotrack.co.uk");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Positions);
    }

    [Fact]
    public async Task GetSeoStats_ShouldHandleEmptyHtmlContent()
    {
        // Arrange
        var mockHtmlFetcher = Substitute.For<IHtmlFetcher>();
        mockHtmlFetcher.FetchHtmlAsync(Arg.Any<string[]>()).Returns(string.Empty);

        var provider = new GoogleSEOStatsProvider(mockHtmlFetcher);

        // Act
        var result = await provider.GetSeoStats(new[] { "land", "registry", "search" }, "infotrack.co.uk");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Positions);
    }

    
    [Fact]
    public async Task GetSeoStats_ShouldReturnEmptyPositions_WhenNoMatchingLinks()
    {
        // Arrange
        var mockHtmlFetcher = Substitute.For<IHtmlFetcher>();
        string testHtml = @"
            <html>
                <body>
                    <a href='https://www.example.com'><h3>Result 1</h3></a>
                    <a href='https://www.example.com'><h3>Result 2</h3></a>
                </body>
            </html>";
        mockHtmlFetcher.FetchHtmlAsync(Arg.Any<string[]>()).Returns(Task.FromResult(testHtml));

        var provider = new GoogleSEOStatsProvider(mockHtmlFetcher);

        // Act
        var result = await provider.GetSeoStats(new[] { "land", "registry", "search" }, "infotrack.co.uk");

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Positions);
    }

    [Fact]
    public async Task GetSeoStats_ShouldReturnAllMatchingPositions()
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

    [Fact]
    public async Task GetSeoStats_ShouldMatchUrlsCaseInsensitively()
    {
        // Arrange
        var mockHtmlFetcher = Substitute.For<IHtmlFetcher>();
        string testHtml = @"
            <html>
                <body>
                    <a href='https://www.INFOtrack.co.uk'><h3>Result 1</h3></a>
                </body>
            </html>";
        mockHtmlFetcher.FetchHtmlAsync(Arg.Any<string[]>()).Returns(Task.FromResult(testHtml));

        var provider = new GoogleSEOStatsProvider(mockHtmlFetcher);

        // Act
        var result = await provider.GetSeoStats(new[] { "land", "registry", "search" }, "infotrack.co.uk");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(new[] { 1 }, result.Positions);
    }

    [Fact]
    public async Task GetSeoStats_ShouldHandleHtmlFetcherException()
    {
        // Arrange
        var mockHtmlFetcher = Substitute.For<IHtmlFetcher>();
        mockHtmlFetcher.FetchHtmlAsync(Arg.Any<string[]>()).Throws(new Exception("Network error"));

        var provider = new GoogleSEOStatsProvider(mockHtmlFetcher);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => provider.GetSeoStats(new[] { "land", "registry", "search" }, "infotrack.co.uk"));
    }

    [Fact]
    public async Task GetSeoStats_IntegrationTest_WithChromeDriver()
    {
        // Arrange
        var driver = WebDriverFactory.CreateChromeDriver();
        var navigator = new GoogleSearchNavigator(driver);
        var htmlFetcher = new GoogleSeleniumHtmlFetcher(navigator);
        var provider = new GoogleSEOStatsProvider(htmlFetcher);

        // Act
        var result = await provider.GetSeoStats(new[] { "land", "registry", "search" }, "infotrack.co.uk");

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Positions);
        Assert.All(result.Positions, position => Assert.True(position > 0));
    }
   

}