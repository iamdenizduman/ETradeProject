namespace Shared.Persistence.Abstracts
{
    public abstract class BaseEntity<TId>
    {
        public TId Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
    }
}
