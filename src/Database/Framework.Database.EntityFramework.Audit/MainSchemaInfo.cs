namespace Framework.Database.EntityFramework.Audit;

public record MainSchemaInfo(string Name)
{
    public static MainSchemaInfo Default = new("App");
}
