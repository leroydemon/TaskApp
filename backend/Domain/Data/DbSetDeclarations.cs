using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data
{
    public partial class TaskAppDbContext
    {
        //It good move for clean code
        public DbSet<TaskToDo> Tasks { get; set; }
    }
}
