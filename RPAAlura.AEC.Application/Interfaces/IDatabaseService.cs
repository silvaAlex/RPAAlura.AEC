namespace RPAAlura.AEC.Application.Interfaces
{
    public interface IDatabaseService
    {
        void CreateDatabaseIfNotExists(string connectionString);
    }
}
