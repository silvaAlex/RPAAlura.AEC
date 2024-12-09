using Microsoft.Extensions.DependencyInjection;
using RPAAlura.AEC.Application.Interfaces;
using RPAAlura.AEC.Application.Services;
using RPAAlura.AEC.Domain.Entities;

namespace RPAAlura.AEC.Console.Handler
{
    public class UrlTaskHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _baseUrl = "https://www.alura.com.br";

        public UrlTaskHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public List<string> GetUrls(string userInput)
        {
            using var scope = _serviceProvider.CreateScope(); // Create a scoped service provider
            var urlService = scope.ServiceProvider.GetService<IRpaTaskService<UrlSearchTaskEntity>>() as RpaTaskGetUrlsService ?? throw new InvalidOperationException("RpaTaskGetUrlsService is not registered or could not be resolved.");

            // Execute the task
            urlService.ExecuteTask(new UrlSearchTaskEntity()
            {
                BaseUrl = _baseUrl,
                NomeCurso = userInput,
                ExecutionTime = TimeSpan.FromSeconds(30)
            });

            return urlService.GetUrls();
        }
    }
}
