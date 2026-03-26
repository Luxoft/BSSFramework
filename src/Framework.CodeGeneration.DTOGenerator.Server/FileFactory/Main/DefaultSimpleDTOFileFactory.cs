using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DefaultSimpleDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DefaultSimpleDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.SimpleDTO;

    public sealed override CodeTypeReference BaseReference => this.IsPersistent() ? this.Configuration.GetBaseAuditPersistentReference() : this.Configuration.GetBaseAbstractReference();

    protected override bool HasToDomainObjectMethod => this.HasMapToDomainObjectMethod && this.IsPersistent();

    protected override bool AllowCreate { get; } = false;


    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        foreach (var baseType in base.GetBaseTypes())
        {
            yield return baseType;
        }

        if (this.IsPersistent())
        {
            if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.IdentityDTO))
            {
                yield return this.GetIdentityObjectContainerTypeReference();
            }
        }
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        if (this.IsPersistent())
        {
            if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.IdentityDTO))
            {
                yield return this.GetIdentityObjectContainerImplementation();
            }

            yield return this.GenerateIdConstructor();
        }
    }

    protected override CodeExpression GetFieldInitExpression(CodeTypeReference codeTypeReference, PropertyInfo property)
    {
        if (!this.CodeTypeReferenceService.IsOptional(property))
        {
            if (property.PropertyType == typeof(Period))
            {
                return typeof(Period).ToTypeReferenceExpression().ToPropertyReference("Eternity");
            }
        }

        return base.GetFieldInitExpression(codeTypeReference, property);
    }
}
