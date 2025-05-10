using InfotrackTest.Google;
using InfotrackTest.Interfaces;
using OpenQA.Selenium;

namespace InfotrackTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Register IWebDriver as a singleton
            builder.Services.AddSingleton<IWebDriver>(provider => WebDriverFactory.CreateChromeDriver());

            // Register ISearchNavigator and its implementation
            builder.Services.AddTransient<ISearchNavigator, GoogleSearchNavigator>();

            // Register IHtmlFetcher and its implementation
            builder.Services.AddTransient<IHtmlFetcher, GoogleSeleniumHtmlFetcher>();

            // Register ISeoStatsProvider and its implementation
            builder.Services.AddTransient<ISeoStatsProvider, GoogleSEOStatsProvider>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            //app.UseHttpsRedirection();

            //app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
