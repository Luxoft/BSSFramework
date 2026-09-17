using FluentMigrator;

namespace SampleSystem.DbGenerate.Migrations;

[Migration(3)]
public class AddAncestorChildViews : Migration
{
    public override void Up()
    {
        this.Execute.Sql(@"
CREATE VIEW [app].[BusinessUnitToAncestorChildView]
AS
SELECT sourceId = [ancestorId], childOrAncestorId = childid
FROM [app].[BusinessUnitAncestorLink]
UNION
SELECT sourceId = [childId], childOrAncestorId = ancestorId
FROM [app].[BusinessUnitAncestorLink]");

        this.Execute.Sql(@"
CREATE VIEW [app].[ManagementUnitToAncestorChildView]
AS
SELECT sourceId = [ancestorId], childOrAncestorId = childid
FROM [app].[ManagementUnitAncestorLink]
UNION
SELECT sourceId = [childId], childOrAncestorId = ancestorId
FROM [app].[ManagementUnitAncestorLink]");

        this.Execute.Sql(@"
CREATE VIEW [app].[LocationToAncestorChildView]
AS
SELECT sourceId = [ancestorId], childOrAncestorId = childid
FROM [app].[LocationAncestorLink]
UNION
SELECT sourceId = [childId], childOrAncestorId = ancestorId
FROM [app].[LocationAncestorLink]");
    }

    public override void Down()
    {
        this.Execute.Sql("DROP VIEW [app].[LocationToAncestorChildView]");
        this.Execute.Sql("DROP VIEW [app].[ManagementUnitToAncestorChildView]");
        this.Execute.Sql("DROP VIEW [app].[BusinessUnitToAncestorChildView]");
    }
}
