using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileFactory._Helpers;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main.Base;
using Framework.Core;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main;

public class DefaultSimpleDTOFileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : MainDTOFileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public override MainDTOFileType FileType { get; } = BaseFileType.SimpleDTO;

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
            if (this.Configuration.GeneratePolicy.Used(this.DomainType, BaseFileType.IdentityDTO))
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
            if (this.Configuration.GeneratePolicy.Used(this.DomainType, BaseFileType.IdentityDTO))
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
