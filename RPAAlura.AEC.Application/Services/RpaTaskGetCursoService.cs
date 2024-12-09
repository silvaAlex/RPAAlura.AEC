using OpenQA.Selenium;
using RPAAlura.AEC.Application.Exceptions;
using RPAAlura.AEC.Application.Helper;
using RPAAlura.AEC.Application.Interfaces;
using RPAAlura.AEC.Domain.Entities;

namespace RPAAlura.AEC.Application.Services
{
    public class RpaTaskGetCursoService : IRpaTaskService<CursoTaskEntity>
    {
        private readonly WebDriverHelper _webDriverHelper;
        private readonly IRpaRepository<Curso> _cursoRepository;

        public RpaTaskGetCursoService(WebDriverHelper webDriverHelper, IRpaRepository<Curso> cursoRepository)
        {
            _webDriverHelper = webDriverHelper;
            _cursoRepository = cursoRepository;
        }


        public void ExecuteTask(CursoTaskEntity task)
        {
            ElementHelper elementHelper = new(_webDriverHelper.GetDriver(), task.ExecutionTime);
            Curso curso = new();

            try
            {
                if (!string.IsNullOrEmpty(task.BaseUrl))
                {
                    _webDriverHelper.NavigateToUrl(task.BaseUrl);
                    elementHelper.WaitForElementVisibility(".formacao-headline-titulo");
                    curso.Titulo = elementHelper.FindElement(".formacao-headline-titulo").Text;
                    curso.CargaHoraria = elementHelper.FindElement(".formacao__info-destaque").Text;
                    curso.Descricao = elementHelper.FindElement(".formacao-sobre-descricao").Text;

                    var nomes = elementHelper.FindElements(".formacao-instrutor-nome");

                    foreach (IWebElement nome in nomes)
                    {
                        if (!string.IsNullOrEmpty(nome.Text))
                        {
                            Professor professor = new()
                            {
                                Nome = nome.Text,
                            };

                            curso.Professores.Add(professor);
                        }
                    }

                    _cursoRepository.Save(curso);
                }
                else
                {
                    throw new RpaTaskException($"Erro ao executar tarefa: url está vazio");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao executar tarefa: {ex.Message}");
                return;
            }
        }
    }
}
