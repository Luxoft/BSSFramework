namespace Framework.ApplicationVariable;

public record ApplicationVariable(string Name, string Description);

public record ApplicationVariable<T>(string Name, string Description, T DefaultValue) : ApplicationVariable(Name, Description);
