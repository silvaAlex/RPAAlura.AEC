using OpenQA.Selenium;

namespace RPAAlura.AEC.Application.Helper
{
    public class WebDriverHelper : IDisposable
    {

        private readonly IWebDriver _webDriver;

        public WebDriverHelper(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public IWebDriver GetDriver() => _webDriver;


        public void NavigateToUrl(string url)
        {
            try
            {
                _webDriver.Navigate().GoToUrl(url);
                _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            }
            catch (WebDriverException ex)
            {
                throw new Exception($"Erro ao navegar para a URL: {url}", ex);
            }
        }

        public void Dispose()
        {
            _webDriver.Quit();
        }
    }
}
