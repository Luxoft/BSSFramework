using Microsoft.Data.SqlClient;

using FluentMigrator;
using FluentMigrator.SqlServer;

using Framework.Configuration.Domain;


namespace SampleSystem.IntegrationTests.__Support
{
    [Migration(2022_11_18_16_09_00)]
    public class InitNumberInDomainObjectEventMigration : Migration
    {
        public override void Up()
        {
            //alter table configuration.DomainObjectEvent add number bigint NOT NULL IDENTITY(1, 1)

            this.Alter.Table(nameof(DomainObjectEvent)).InSchema(nameof(Configuration))
                .AddColumn("number").AsInt64().Identity(1, 1).NotNullable();
        }

        public override void Down()
        {
        }
    }
}
