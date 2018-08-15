namespace TesteSuperbid.WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Balance = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Ammount = c.Double(nullable: false),
                        Pending = c.Boolean(nullable: false),
                        AccountFrom_Id = c.Int(),
                        AccountTo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountFrom_Id)
                .ForeignKey("dbo.Accounts", t => t.AccountTo_Id)
                .Index(t => t.AccountFrom_Id)
                .Index(t => t.AccountTo_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "AccountTo_Id", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "AccountFrom_Id", "dbo.Accounts");
            DropIndex("dbo.Transactions", new[] { "AccountTo_Id" });
            DropIndex("dbo.Transactions", new[] { "AccountFrom_Id" });
            DropTable("dbo.Transactions");
            DropTable("dbo.Accounts");
        }
    }
}
