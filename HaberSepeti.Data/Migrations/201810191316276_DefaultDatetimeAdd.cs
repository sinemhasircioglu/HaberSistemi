namespace HaberSepeti.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultDatetimeAdd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "CreatedDate", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.Comments", "CreatedDate", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.Users", "CreatedDate", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Comments", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.News", "CreatedDate", c => c.DateTime(nullable: false));
        }
    }
}
