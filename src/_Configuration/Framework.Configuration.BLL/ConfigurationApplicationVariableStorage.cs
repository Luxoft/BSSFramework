using Framework.ApplicationVariable;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.GenericQueryable;
using SecuritySystem;

namespace Framework.Configuration.BLL;

public class ConfigurationApplicationVariableStorage(
    [DisabledSecurity] IRepository<SystemConstant> systemConstantRepository,
    IConfigurationBLLContext context) : IApplicationVariableStorage
{
    public async Task<T> GetValueAsync<T>(ApplicationVariable<T> variable, CancellationToken cancellationToken = default)
    {
        var systemConstant = await systemConstantRepository.GetQueryable().GenericSingleAsync(sc => sc.Code == variable.Name, cancellationToken);

        return context.SystemConstantSerializerFactory.Create<T>().Parse(systemConstant.Value);
    }

    public async Task<Dictionary<ApplicationVariable.ApplicationVariable, string>> GetVariablesAsync(
        CancellationToken cancellationToken = default)
    {
        var dbList = await systemConstantRepository.GetQueryable().GenericToListAsync(cancellationToken);

        return dbList.ToDictionary(sc => new ApplicationVariable.ApplicationVariable(sc.Code, sc.Description), sc => sc.Value);
    }

    public async Task UpdateVariableAsync(string variableName, string newRawValue, CancellationToken cancellationToken = default)
    {
        var systemConstant = await systemConstantRepository.GetQueryable().GenericSingleAsync(sc => sc.Code == variableName, cancellationToken);

        if (systemConstant.Value != newRawValue)
        {
            systemConstant.Value = newRawValue;
            systemConstant.IsManual = true;

            var type = context.SystemConstantTypeResolver.Resolve(systemConstant.Type.FullTypeName);

            await new Func<SystemConstant, CancellationToken, Task>(this.UpdateVariableAsync<object>)
                  .CreateGenericMethod(type)
                  .Invoke<Task>(this, systemConstant, cancellationToken);
        }
    }

    public async Task UpdateVariableAsync<T>(SystemConstant systemConstant, CancellationToken cancellationToken = default)
    {
        context.SystemConstantSerializerFactory.Create<T>().Parse(systemConstant.Value);

        await systemConstantRepository.SaveAsync(systemConstant, cancellationToken);
    }
}
