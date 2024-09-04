using Domain.Interfaces;

namespace Domain.Entities
{
    public class User : IEntity
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime CreatedDate {  get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate {  get; set; } = DateTime.UtcNow;
    }
}
