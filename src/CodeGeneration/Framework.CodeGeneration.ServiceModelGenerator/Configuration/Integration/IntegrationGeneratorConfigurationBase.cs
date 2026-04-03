using System.CodeDom;

using Framework.BLL.Domain.DirectMode;
using Framework.CodeGeneration.BLLCoreGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Integration.Remove;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Integration.Save;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Integration.Save.ByModel;
using Framework.FileGeneration.Configuration;
using Framework.Projection;

using SecuritySystem;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.Integration;

public abstract class IntegrationGeneratorConfigurationBase<TEnvironment>(TEnvironment environment)
    : ServiceModelGeneratorBase<TEnvironment>(environment), IIntegrationGeneratorConfiguration<TEnvironment>
    where TEnvironment : class, IServiceModelGenerationEnvironment
{
    public override string ImplementClassName { get; } = "IntegrationFacade";

    protected override string NamespacePostfix { get; } = "ServiceFacade.Integration";

    public virtual string InsertMethodName { get; } = "Insert";

    public virtual string SaveMethodName { get; } = "Save";

    public virtual CodeExpression IntegrationSecurityRule =>
        this.Environment.BLLCore.GetSecurityCodeExpression(SecurityRole.SystemIntegration);

    protected override IEnumerable<Type> GetDomainTypes() => base.GetDomainTypes().Where(domainType => !domainType.IsProjection());

    public override IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType)
    {
        {
            var singleSaveGenerator = new IntegrationSaveMethodGenerator<IntegrationGeneratorConfigurationBase<TEnvironment>>(this, domainType);

            yield return singleSaveGenerator;

            yield return new IntegrationSaveManyMethodGenerator<IntegrationGeneratorConfigurationBase<TEnvironment>>(singleSaveGenerator);

            yield return new IntegrationRemoveMethodGenerator<IntegrationGeneratorConfigurationBase<TEnvironment>>(this, domainType);
        }


        foreach (var integrationSaveModelType in this.Environment.GetModelTypes(domainType, this.Environment.BLLCore.IntegrationSaveModelType!))
        {
            integrationSaveModelType.CheckDirectMode(DirectMode.In, true);

            yield return new IntegrationSaveModelMethodGenerator<IntegrationGeneratorConfigurationBase<TEnvironment>>(this, domainType, integrationSaveModelType);
        }
    }
}
