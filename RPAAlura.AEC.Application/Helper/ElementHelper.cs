using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;

namespace RPAAlura.AEC.Application.Helper
{
    public class ElementHelper
    {
        private readonly WebDriverWait _wait;

        public ElementHelper(IWebDriver webDriver, TimeSpan executionTime)
        { 
            _wait = new WebDriverWait(webDriver, executionTime);
        }

        public void WaitForElementToBeClickable(By selector)
        {
            try
            {
                _wait.Until(driver => driver.FindElement(selector).Enabled && driver.FindElement(selector).Displayed);
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine($"Tempo de espera excedido para o elemento clicável: {selector}");
                throw new Exception("Erro ao esperar elemento clicável.", ex);
            }
        }

        public void WaitForElementVisibility(string selector)
        {
            try
            {
                _wait.Until(ExpectedConditions.ElementExists(By.CssSelector(selector)));
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine($"Tempo de espera excedido para o elemento: {selector}");
                throw new Exception("Erro ao esperar elemento visível.", ex); ;
            }
        }

        public void WaitForElementVisibility(By selector)
        {
            try
            {
                _wait.Until(driver => driver.FindElement(selector).Displayed);
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine($"Tempo de espera excedido para o elemento: {selector}");
                throw new Exception("Erro ao esperar elemento visível.", ex);
            }
        }

        public void EnterText(By selector, string text)
        {
            try
            {
                var element = _wait.Until(driver => driver.FindElement(selector));
                element.Clear();
                element.SendKeys(text);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao inserir texto no elemento: {selector}", ex);
            }
        }

        public void ClickElement(By selector)
        {
            try
            {
                var element = _wait.Until(driver => driver.FindElement(selector));
                element.Click();
            }
            catch (ElementClickInterceptedException ex)
            {
                throw new Exception($"Erro ao clicar no elemento: {selector}", ex);
            }
        }

        public IWebElement FindElement(string selector)
        {
            try
            {
                return _wait.Until(driver => driver.FindElement(By.CssSelector(selector)));
            }
            catch (NoSuchElementException ex)
            {
                throw new Exception($"Elemento não encontrado: {selector}", ex);
            }
        }

        public ReadOnlyCollection<IWebElement> FindElements(string selector)
        {
            try
            {
                return _wait.Until(driver => driver.FindElements(By.CssSelector(selector)));
            }
            catch (NoSuchElementException ex)
            {
                throw new Exception($"Elemento não encontrado: {selector}", ex);
            }
        }
    }
}
