namespace AwesomeTodo.DataAccess.Migrations
{
    using AwesomeTodo.DataAccess.Models;
    using System;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AwesomeTodo.DataAccess.AwesomeTodoDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AwesomeTodo.DataAccess.AwesomeTodoDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            var todos = new Collection<TodoItem>
            {
                new TodoItem { Id = 1, Title = "My first todo item" },
                new TodoItem { Id = 2, Title = "My second todo item" },
                new TodoItem { Id = 3, Title = "My third todo item" },
                new TodoItem { Id = 4, Title = "My fourth todo item" }
            };

            var events = new Collection<CalendarEvent>
            {
                new CalendarEvent { Id = 1, Title = "Coffee", StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) },
                new CalendarEvent { Id = 2, Title = "Gym", StartTime = DateTime.Now.AddHours(2), EndTime = DateTime.Now.AddHours(3) },
                new CalendarEvent { Id = 3, Title = "Watch TV", StartTime = DateTime.Now.AddHours(10), EndTime = DateTime.Now.AddHours(12) },

                new CalendarEvent { Id = 4, Title = "Planning", StartTime = DateTime.Now.AddDays(2), EndTime = DateTime.Now.AddDays(2).AddHours(1) },
                new CalendarEvent { Id = 5, Title = "Drink water", StartTime = DateTime.Now.AddDays(2).AddHours(2), EndTime = DateTime.Now.AddDays(2).AddHours(3) },
                new CalendarEvent { Id = 6, Title = "Cook", StartTime = DateTime.Now.AddDays(2).AddHours(10), EndTime = DateTime.Now.AddDays(2).AddHours(12) },

                new CalendarEvent { Id = 7, Title = "Groceries", StartTime = DateTime.Now.AddDays(5), EndTime = DateTime.Now.AddDays(5).AddHours(1) },
                new CalendarEvent { Id = 8, Title = "Walk dog", StartTime = DateTime.Now.AddDays(5).AddHours(2), EndTime = DateTime.Now.AddDays(5).AddHours(3) },

                new CalendarEvent { Id = 9, Title = "Draw", StartTime = DateTime.Now.AddDays(13).AddHours(10), EndTime = DateTime.Now.AddDays(13).AddHours(12) }
            };

            foreach (var item in todos)
                context.Todos.AddOrUpdate(t => t.Id, item);

            foreach (var item in events)
                context.CalendarEvents.AddOrUpdate(e => e.Id, item);

            context.SaveChanges();
         }
    }
}
