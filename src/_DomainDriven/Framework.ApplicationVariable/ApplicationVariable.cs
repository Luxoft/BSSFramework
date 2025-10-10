namespace Framework.ApplicationVariable;

public record ApplicationVariable(string Name)
{
    public required string Description { get; init; }
}

public record ApplicationVariable<T>(string Name, T DefaultValue) : ApplicationVariable(Name);
