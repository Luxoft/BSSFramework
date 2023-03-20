using System;

using Framework.Core;
using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.ServiceModelGenerator;

/// <summary>
/// Генератор фасадного метода по модели
/// </summary>
/// <typeparam name="TConfiguration"></typeparam>
/// <typeparam name="TBLLRoleAttribute"></typeparam>
public abstract class ModelMethodGenerator<TConfiguration, TBLLRoleAttribute> : MethodGenerator<TConfiguration, TBLLRoleAttribute>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
        where TBLLRoleAttribute : BLLServiceRoleAttribute, new()
{
    protected readonly Type ModelType;


    protected ModelMethodGenerator(TConfiguration configuration, Type domainType, Type modelType)
            : base(configuration, domainType)
    {
        if (modelType == null) throw new ArgumentNullException(nameof(modelType));

        this.ModelType = modelType;
    }


    protected override TBLLRoleAttribute Attribute =>

            this.ModelType.GetCustomAttribute<TBLLRoleAttribute>() ?? new TBLLRoleAttribute();

    protected sealed override DBSessionMode SessionMode =>

            this.ModelType.GetCustomAttribute<DBSessionModeAttribute>().Maybe(attr => attr.SessionMode, () => base.SessionMode);
}
