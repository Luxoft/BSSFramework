using CommonFramework;

using Framework.Application.ApplicationVariable;
using Framework.Application.Repository;
using Framework.Configuration.Domain;

using GenericQueryable;

using SecuritySystem.Attributes;

namespace Framework.Configuration.BLL;

public class ConfigurationApplicationVariableStorage(
    [DisabledSecurity] IRepository<SystemConstant> systemConstantRepository,
    IConfigurationBLLContext context) : IApplicationVariableStorage
{
    public async Task<T> GetValueAsync<T>(ApplicationVariable<T> variable, CancellationToken cancellationToken = default)
    {
        var systemConstant = await systemConstantRepository.GetQueryable().GenericSingleAsync(services => services.Code == variable.Name, cancellationToken);

        return context.SystemConstantSerializerFactory.Create<T>().Parse(systemConstant.Value);
    }

    public async Task<Dictionary<ApplicationVariable, string>> GetVariablesAsync(
        CancellationToken cancellationToken = default)
    {
        var dbList = await systemConstantRepository.GetQueryable().GenericToListAsync(cancellationToken);

        return dbList.ToDictionary(sc => new ApplicationVariable(sc.Code) { Description = sc.Description }, services => services.Value);
    }

    public async Task UpdateVariableAsync(string variableName, string newRawValue, CancellationToken cancellationToken = default)
    {
        var systemConstant = await systemConstantRepository.GetQueryable().GenericSingleAsync(services => services.Code == variableName, cancellationToken);

        if (systemConstant.Value != newRawValue)
        {
            systemConstant.Value = newRawValue;
            systemConstant.IsManual = true;

            var type = context.TargetSystemTypeResolver.Resolve(systemConstant.Type.FullTypeName);

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
