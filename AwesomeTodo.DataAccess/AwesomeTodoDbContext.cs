using AwesomeTodo.DataAccess.Models;
using System.Data.Entity;

namespace AwesomeTodo.DataAccess
{
    public class AwesomeTodoDbContext : DbContext
    {
        public DbSet<TodoItem> Todos { get; set; }
        public DbSet<CalendarEvent> CalendarEvents { get; set; }

        public AwesomeTodoDbContext() : base("AwesomeTodoDB")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
