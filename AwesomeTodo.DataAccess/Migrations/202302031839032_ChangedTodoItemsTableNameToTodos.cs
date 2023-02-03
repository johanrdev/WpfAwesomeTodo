namespace AwesomeTodo.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTodoItemsTableNameToTodos : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TodoItems", newName: "Todos");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Todos", newName: "TodoItems");
        }
    }
}
