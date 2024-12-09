using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace RPAAlura.AEC.Application.Factory
{
    public class WebDriverFactory
    {
        public static IWebDriver CreateWebDriver(bool isHeadless = false)
        {

            var seleniumUrl = Environment.GetEnvironmentVariable("SELENIUM_URL")
                          ?? "http://localhost:4444";


            var options = new ChromeOptions();
            if (isHeadless)
            {
                options.AddArgument("--headless");
            }

            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            try
            {
                return new RemoteWebDriver(new Uri(seleniumUrl), options.ToCapabilities());
            }
            catch (WebDriverException ex)
            {
                throw new InvalidOperationException("Erro ao iniciar o WebDriver. Certifique-se de que o ChromeDriver está configurado corretamente.", ex);
            }
        }
    }
}
