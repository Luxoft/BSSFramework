using Anch.Core;

using Framework.BLL.Domain.ServiceRole;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.Core;
using Framework.Database;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators;

/// <summary>
/// Генератор фасадного метода по модели
/// </summary>
/// <typeparam name="TConfiguration"></typeparam>
/// <typeparam name="TBLLRoleAttribute"></typeparam>
public abstract class ModelMethodGenerator<TConfiguration, TBLLRoleAttribute>(TConfiguration configuration, Type domainType, Type modelType)
    : MethodGenerator<TConfiguration, TBLLRoleAttribute>(configuration, domainType)
    where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
    where TBLLRoleAttribute : BLLServiceRoleAttribute, new()
{
    protected readonly Type ModelType = modelType ?? throw new ArgumentNullException(nameof(modelType));

    protected override TBLLRoleAttribute Attribute =>

        this.ModelType.GetCustomAttribute<TBLLRoleAttribute>() ?? new TBLLRoleAttribute();

    protected sealed override DBSessionMode SessionMode =>

        this.ModelType.GetCustomAttribute<DBSessionModeAttribute>().Maybe(attr => attr.SessionMode, () => base.SessionMode);
}

