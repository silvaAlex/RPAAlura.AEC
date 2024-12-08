namespace RPAAlura.AEC.Application.Interfaces
{
    public interface IRpaTaskService<T> where T : class
    {
        void ExecuteTask(T task);
    }
}
