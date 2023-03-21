using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

using Framework.CodeDom;
using Framework.CodeDom.TypeScript;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Main;

/// <summary>
/// Default simpleDTO file factory
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class DefaultSimpleDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public DefaultSimpleDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }

    public override MainDTOFileType FileType => DTOGenerator.FileType.SimpleDTO;

    public override CodeTypeReference BaseReference
        => this.Configuration.GetCodeTypeReference(
                                                   this.DomainType,
                                                   this.IsPersistent() ? DTOGenerator.FileType.BaseAuditPersistentDTO : DTOGenerator.FileType.BaseAbstractDTO);

    protected override CodeExpression GetFieldInitExpression(PropertyInfo property)
    {
        if (this.CodeTypeReferenceService.IsOptional(property))
        {
            return base.GetFieldInitExpression(property);
        }

        if (property.PropertyType.IsPeriod())
        {
            return typeof(Period).ToTypeReferenceExpression().ToPropertyReference("Eternity");
        }

        return base.GetFieldInitExpression(property);
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
        }
    }
}
