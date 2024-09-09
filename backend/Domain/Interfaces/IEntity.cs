namespace Domain.Interfaces
{
    // Interface representing a basic entity with identification and timestamps
    public interface IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
