namespace AwesomeTodo.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPropertyIsCompletedInTodoItemModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TodoItems", "IsCompleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TodoItems", "IsCompleted");
        }
    }
}
