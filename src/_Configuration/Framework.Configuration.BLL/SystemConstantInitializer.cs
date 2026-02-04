using System.Reflection;

using CommonFramework;

using Framework.ApplicationVariable;
using Framework.Configuration.Domain;
using Framework.DomainDriven.Repository;

using GenericQueryable;

namespace Framework.Configuration.BLL;

public class SystemConstantInitializer(IConfigurationBLLContext context, IRepository<SystemConstant> repository, IEnumerable<SystemConstantInfo> infoList) : ISystemConstantInitializer
{
    public async Task Initialize(CancellationToken cancellationToken)
    {
        var dbConstants = await repository.GetQueryable().GenericToListAsync(cancellationToken);

        var initMethod = new Func<ApplicationVariable<object>, IReadOnlyList<SystemConstant>, CancellationToken, Task>(this.Initialize).Method.GetGenericMethodDefinition();

        foreach (var systemConstantInfo in infoList)
        {
            foreach (var field in systemConstantInfo.SystemConstantContainerType.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (field.GetValue(null) is { } systemConstant
                    && systemConstant.GetType().GetGenericTypeImplementationArgument(typeof(ApplicationVariable<>)) is { } constType)
                {
                    await initMethod.MakeGenericMethod(constType).Invoke<Task>(this, systemConstant, dbConstants, cancellationToken);
                }
            }
        }
    }

    private async Task Initialize<T>(ApplicationVariable<T> typedSystemConstant, IReadOnlyList<SystemConstant> dbConstants, CancellationToken cancellationToken)
    {
        var systemConstant = dbConstants.SingleOrDefault(sc => string.Equals(sc.Code, typedSystemConstant.Name, StringComparison.CurrentCultureIgnoreCase))
                             ?? new SystemConstant { Code = typedSystemConstant.Name, Type = context.Logics.DomainType.GetByType(typeof(T)) };

        if (!systemConstant.IsManual)
        {
            var serializer = context.SystemConstantSerializerFactory.Create<T>();

            var serializedValue = serializer.Format(typedSystemConstant.DefaultValue);

            if (systemConstant.IsNew || (systemConstant.Description != typedSystemConstant.Description || systemConstant.Value != serializedValue))
            {
                systemConstant.Description = typedSystemConstant.Description;
                systemConstant.Value = serializedValue;

                await repository.SaveAsync(systemConstant, cancellationToken);
            }
        }
    }
}
