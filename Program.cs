using InfotrackTest;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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
            builder.Services.AddSingleton<IWebDriver>(provider =>
            {
                var options = new ChromeOptions();
                options.AddArgument("--disable-gpu");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--profile-directory=Default");

                var service = ChromeDriverService.CreateDefaultService("C:\\Program Files\\Selenium\\chromedriver-win64\\");
                return new ChromeDriver(service, options);
            });

            // Register IHtmlFetcher and its implementation
            builder.Services.AddTransient<IHtmlFetcher, SeleniumHtmlFetcher>();

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
