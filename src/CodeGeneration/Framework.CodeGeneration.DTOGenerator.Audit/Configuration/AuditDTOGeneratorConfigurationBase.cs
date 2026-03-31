using Framework.CodeGeneration.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Audit.Configuration;

public abstract class AuditDTOGeneratorConfigurationBase<TEnvironment>(TEnvironment environment)
    : CodeGeneratorConfiguration<TEnvironment>(environment), IAuditDTOGeneratorConfiguration<TEnvironment>
    where TEnvironment : class, IAuditDTOGenerationEnvironment
{
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
