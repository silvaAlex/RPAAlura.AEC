using OpenQA.Selenium;
using RPAAlura.AEC.Application.Exceptions;
using RPAAlura.AEC.Application.Helper;
using RPAAlura.AEC.Application.Interfaces;
using RPAAlura.AEC.Domain.Entities;
using System.Collections.ObjectModel;

namespace RPAAlura.AEC.Application.Services
{
    public class RpaTaskGetUrlsService : IRpaTaskService<UrlSearchTaskEntity>
    {
        private readonly WebDriverHelper _webDriverHelper;
        private UrlAlura _alura;

        public RpaTaskGetUrlsService(WebDriverHelper webDriverHelper)
        {
            _webDriverHelper = webDriverHelper;
            _alura = new UrlAlura();
        }

        public void ExecuteTask(UrlSearchTaskEntity task)
        {
            ElementHelper helper = new(_webDriverHelper.GetDriver(), task.ExecutionTime);

            try
            {
                if (!string.IsNullOrEmpty(task.BaseUrl))
                {
                    _webDriverHelper.NavigateToUrl(task.BaseUrl);
                    helper.WaitForElementVisibility(By.Id("header-barraBusca-form-campoBusca"));
                    helper.EnterText(By.Id("header-barraBusca-form-campoBusca"), task.NomeCurso);
                    helper.WaitForElementToBeClickable(By.ClassName("header__nav--busca-submit"));
                    helper.ClickElement(By.ClassName("header__nav--busca-submit"));

                    helper.WaitForElementVisibility(".busca-resultado-link");

                    var links = helper.FindElements(".busca-resultado-link");

                    AddUrls(links);
                }
                else
                {
                    throw new RpaTaskException($"Erro ao executar a tarefa: task.NomeCurso está vazio");
                }
            }
            catch (Exception ex)
            {
                throw new RpaTaskException($"Erro ao executar a tarefa: {ex.Message}");
            }
        }

        private void AddUrls(ReadOnlyCollection<IWebElement> links)
        {
            foreach (IWebElement link in links)
            {
                string href = link.GetDomAttribute("href");
                if (!string.IsNullOrEmpty(href))
                {
                    _alura.AddUrl(href);
                }
            }
        }

        public List<string> GetUrls()
        {
            return _alura.Urls;
        }
    }
}
