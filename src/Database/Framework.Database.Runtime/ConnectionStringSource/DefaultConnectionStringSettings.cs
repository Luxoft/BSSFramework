namespace Framework.Database.ConnectionStringSource;

public record DefaultConnectionStringSettings(string Name)
{
    public static DefaultConnectionStringSettings Default { get; } = new("DefaultConnection");
}
