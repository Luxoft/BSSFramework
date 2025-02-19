namespace Framework.ApplicationVariable;

public interface IApplicationVariableStorage
{
    T GetValue<T>(ApplicationVariable<T> variable) => this.GetValueAsync(variable).GetAwaiter().GetResult();

    Task<T> GetValueAsync<T>(ApplicationVariable<T> variable, CancellationToken cancellationToken = default);

    Task<Dictionary<ApplicationVariable, string>> GetVariablesAsync(CancellationToken cancellationToken = default);

    Task UpdateVariableAsync(string variableName, string newRawValue, CancellationToken cancellationToken = default);
}
