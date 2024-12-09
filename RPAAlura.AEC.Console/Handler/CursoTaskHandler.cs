using Microsoft.Extensions.DependencyInjection;
using RPAAlura.AEC.Application.Interfaces;
using RPAAlura.AEC.Application.Services;
using RPAAlura.AEC.Domain.Entities;

namespace RPAAlura.AEC.Console.Handler
{
    public class CursoTaskHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _baseUrl = "https://www.alura.com.br";

        public CursoTaskHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void ExecuteCursoTasks(List<string> urls)
        {

            using var scope = _serviceProvider.CreateScope(); // Create a scoped service provider
            var cursoService = scope.ServiceProvider.GetService<IRpaTaskService<CursoTaskEntity>>() as RpaTaskGetCursoService ?? throw new InvalidOperationException("RpaTaskGetCursoService is not registered or could not be resolved.");

            if (urls != null && urls.Count > 0)
            {
                foreach (string url in urls)
                {
                    cursoService?.ExecuteTask(new CursoTaskEntity()
                    {
                        BaseUrl = url.Contains(_baseUrl) ? url : $"{_baseUrl}{url}",
                        ExecutionTime = TimeSpan.FromSeconds(30),
                    });
                }
            }
        }
    }
}
