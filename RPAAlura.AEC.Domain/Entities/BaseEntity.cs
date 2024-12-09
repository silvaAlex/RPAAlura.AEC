namespace RPAAlura.AEC.Domain.Entities
{
    public abstract class BaseEntity
    {
        public string BaseUrl { get; set; } = string.Empty;
        public TimeSpan ExecutionTime { get; set; }
    }
}
