using FluentMigrator;

namespace SampleSystem.DbMigrator
{
    [Migration(2)]
    public class AddLogTable : Migration
    {
        public override void Up()
        {
            this.Create.Table("Log")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Text").AsString();
        }

        public override void Down()
        {
            this.Delete.Table("Log");
        }
    }


    [Migration(3)]
    public class AddLogTable2 : Migration
    {
        public override void Up()
        {
            this.Create.Table("Log2")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Text").AsString();
        }

        public override void Down()
        {
            this.Delete.Table("Log2");
        }
    }
}
