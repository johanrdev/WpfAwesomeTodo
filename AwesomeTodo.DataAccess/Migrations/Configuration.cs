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

            foreach (var todo in todos)
                context.Todos.AddOrUpdate(t => t.Id, todo);

            context.SaveChanges();
         }
    }
}
