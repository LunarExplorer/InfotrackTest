using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace InfotrackTest
{
    public static class WebDriverFactory
    {
        public static IWebDriver CreateChromeDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--profile-directory=Default");

            var service = ChromeDriverService.CreateDefaultService("C:\\Program Files\\Selenium\\chromedriver-win64\\");
            return new ChromeDriver(service, options);
        }
    }
}
