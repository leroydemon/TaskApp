using Domain.Enums;
using Domain.SortableFields;

namespace Domain.Filters
{
    public class TaskToDoFilter : FilterBase<TaskToDoSortableFields>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public StatusEnum Status { get; set; }
        public PriorityEnum Priority { get; set; }
    }
}
