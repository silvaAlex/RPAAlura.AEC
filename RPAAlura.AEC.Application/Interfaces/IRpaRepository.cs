namespace RPAAlura.AEC.Application.Interfaces
{
    public interface IRpaRepository<T> where T : class
    {
        void Save(T task);
    }
}
