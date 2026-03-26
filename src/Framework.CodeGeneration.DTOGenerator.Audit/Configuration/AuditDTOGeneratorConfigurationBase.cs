using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Audit;

public abstract class AuditDTOGeneratorConfigurationBase<TEnvironment> : GeneratorConfiguration<TEnvironment>, IAuditDTOGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IAuditDTOGenerationEnvironmentBase
{
    protected AuditDTOGeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
    {
    }

    public virtual string DomainObjectPropertiesRevisionDTOFullTypeName => $"{this.Namespace}.{this.DomainObjectPropertiesRevisionDTOTypeName}";

    public virtual string DomainObjectPropertiesRevisionDTOTypeName => $"{this.DomainObjectPropertyRevisionsDTOPrefixName}DomainObjectPropertiesRevision{this.DomainObjectPropertyRevisionsDTOPostfixName}DTO";

    public virtual string PropertyRevisionTypeName => $"{this.PropertyRevisionDTOPrefixName}PropertyRevision{this.PropertyRevisionDTOPostfixName}DTO";

    public virtual string PropertyRevisionFullTypeName => $"{this.Namespace}.{this.PropertyRevisionTypeName}";

    protected abstract string DomainObjectPropertyRevisionsDTOPrefixName { get; }

    protected virtual string DomainObjectPropertyRevisionsDTOPostfixName => string.Empty;

    protected abstract string PropertyRevisionDTOPrefixName { get; }

    protected virtual string PropertyRevisionDTOPostfixName => string.Empty;

    protected override string NamespacePostfix { get; } = "Generated.DTO";
}
