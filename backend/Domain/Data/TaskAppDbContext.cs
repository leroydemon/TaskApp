using Domain.DbConfiguration;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data
{
    //Db settings
    public partial class TaskAppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public TaskAppDbContext(DbContextOptions<TaskAppDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EntityConfiguration());
            modelBuilder.ApplyConfiguration(new TaskToDoConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
