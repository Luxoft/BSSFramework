namespace Framework.Application.ApplicationVariable;

public interface IApplicationVariableStorage
{
    Task<T> GetValueAsync<T>(ApplicationVariable<T> variable, CancellationToken ct);

    Task<Dictionary<ApplicationVariable, string>> GetVariablesAsync(CancellationToken ct);

    Task UpdateVariableAsync(string variableName, string newRawValue, CancellationToken ct);
}
