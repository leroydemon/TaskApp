using Domain.Interfaces;

namespace Domain.Entities
{
    public class Entity : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate {  get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate {  get; set; } = DateTime.UtcNow;
    }
}
