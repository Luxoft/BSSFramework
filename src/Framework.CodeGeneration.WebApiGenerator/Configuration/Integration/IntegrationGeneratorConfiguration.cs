using System.CodeDom;

using Framework.BLL.Domain.DirectMode;
using Framework.CodeGeneration.BLLCoreGenerator.Extensions;
using Framework.CodeGeneration.DomainMetadata;
using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;
using Framework.CodeGeneration.WebApiGenerator.MethodGenerators._Base;
using Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Integration.Remove;
using Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Integration.Save;
using Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Integration.Save.ByModel;
using Framework.Projection;

using SecuritySystem;

namespace Framework.CodeGeneration.WebApiGenerator.Configuration.Integration;

public abstract class IntegrationGeneratorConfigurationBase<TEnvironment> : GeneratorConfigurationBase<TEnvironment>, IIntegrationGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IGenerationEnvironmentBase
{
    protected IntegrationGeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
    {
    }


    public override string ImplementClassName { get; } = "IntegrationFacade";

    protected override string NamespacePostfix { get; } = "ServiceFacade.Integration";

    public virtual string InsertMethodName { get; } = "Insert";

    public virtual string SaveMethodName { get; } = "Save";

    public virtual CodeExpression IntegrationSecurityRule =>
        this.Environment.BLLCore.GetSecurityCodeExpression(SecurityRole.SystemIntegration);

    protected override IEnumerable<Type> GetDomainTypes()
    {
        return base.GetDomainTypes().Where(domainType => !domainType.IsProjection());
    }

    public override IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType)
    {
        {
            var singleSaveGenerator = new IntegrationSaveMethodGenerator<IntegrationGeneratorConfigurationBase<TEnvironment>>(this, domainType);

            yield return singleSaveGenerator;

            yield return new IntegrationSaveManyMethodGenerator<IntegrationGeneratorConfigurationBase<TEnvironment>>(singleSaveGenerator);

            yield return new IntegrationRemoveMethodGenerator<IntegrationGeneratorConfigurationBase<TEnvironment>>(this, domainType);
        }


        foreach (var integrationSaveModelType in this.Environment.GetModelTypes(domainType, this.Environment.BLLCore.IntegrationSaveModelType))
        {
            integrationSaveModelType.CheckDirectMode(DirectMode.In, true);

            yield return new IntegrationSaveModelMethodGenerator<IntegrationGeneratorConfigurationBase<TEnvironment>>(this, domainType, integrationSaveModelType);
        }
    }
}
