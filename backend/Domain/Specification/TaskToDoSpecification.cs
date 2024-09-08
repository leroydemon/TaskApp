using Domain.Entities;
using Domain.Enums;
using Domain.Filters;
using Domain.SortableFields;
using Domain.Specialization;
using System.Linq.Expressions;

namespace Domain.Specification
{
    public class TaskToDoSpecification : SpecificationBase<TaskToDo>
    {
        public TaskToDoSpecification(TaskToDoFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Title))
            {
                ApplyFilter(t => t.Title.Contains(filter.Title));
            }

            if (!string.IsNullOrEmpty(filter.Description))
            {
                ApplyFilter(t => t.Description.Contains(filter.Description));
            }

            if (filter.DueDate != default(DateTime))
            {
                ApplyFilter(t => t.DueDate == filter.DueDate);
            }

            if (filter.Status != default(StatusEnum))
            {
                ApplyFilter(t => t.Status == filter.Status);
            }

            if (filter.Priority != default(PriorityEnum))
            {
                ApplyFilter(t => t.Priority == filter.Priority);
            }

            ApplySorting(filter.OrderBy, filter.Ascending);
            ApplyPaging(filter.Skip, filter.Take);
        }

        private void ApplySorting(TaskToDoSortableFields sortBy, OrderByDirection ascending)
        {
            Expression<Func<TaskToDo, object>> orderByExpression = sortBy switch
            {
                TaskToDoSortableFields.Title => t => t.Title,
                TaskToDoSortableFields.DueDate => t => t.DueDate,
                TaskToDoSortableFields.Status => t => t.Status,
                TaskToDoSortableFields.Priority => t => t.Priority,
                _ => t => t.Id
            };

            if (ascending == OrderByDirection.Ascending)
            {
                ApplyOrderBy(orderByExpression);
            }
            else
            {
                ApplyOrderByDescending(orderByExpression);
            }
        }
    }

}
