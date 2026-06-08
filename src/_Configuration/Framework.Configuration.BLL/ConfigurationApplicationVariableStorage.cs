using Anch.Core;
using Anch.GenericQueryable;
using Anch.SecuritySystem.Attributes;

using Framework.Application.ApplicationVariable;
using Framework.Application.Repository;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public class ConfigurationApplicationVariableStorage(
    [DisabledSecurity] IRepository<SystemConstant> systemConstantRepository,
    IConfigurationBLLContext context) : IApplicationVariableStorage
{
    public async Task<T> GetValueAsync<T>(ApplicationVariable<T> variable, CancellationToken ct)
    {
        var systemConstant = await systemConstantRepository.GetQueryable().GenericSingleAsync(services => services.Code == variable.Name, ct);

        return context.SystemConstantSerializerFactory.Create<T>().Parse(systemConstant.Value);
    }

    public async Task<Dictionary<ApplicationVariable, string>> GetVariablesAsync(CancellationToken ct)
    {
        var dbList = await systemConstantRepository.GetQueryable().GenericToListAsync(ct);

        return dbList.ToDictionary(sc => new ApplicationVariable(sc.Code) { Description = sc.Description }, services => services.Value);
    }

    public async Task UpdateVariableAsync(string variableName, string newRawValue, CancellationToken ct)
    {
        var systemConstant = await systemConstantRepository.GetQueryable().GenericSingleAsync(services => services.Code == variableName, ct);

        if (systemConstant.Value != newRawValue)
        {
            systemConstant.Value = newRawValue;
            systemConstant.IsManual = true;

            var type = context.TargetSystemTypeResolver.Resolve(systemConstant.Type.FullTypeName);

            await new Func<SystemConstant, CancellationToken, Task>(this.UpdateVariableAsync<object>)
                  .CreateGenericMethod(type)
                  .Invoke<Task>(this, systemConstant, ct);
        }
    }

    public async Task UpdateVariableAsync<T>(SystemConstant systemConstant, CancellationToken ct)
    {
        context.SystemConstantSerializerFactory.Create<T>().Parse(systemConstant.Value);

        await systemConstantRepository.SaveAsync(systemConstant, ct);
    }
}

