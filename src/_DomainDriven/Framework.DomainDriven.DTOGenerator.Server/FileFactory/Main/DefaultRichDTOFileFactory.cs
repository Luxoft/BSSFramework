using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DefaultRichDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DefaultRichDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.RichDTO;


    protected override bool HasToDomainObjectMethod => this.HasMapToDomainObjectMethod;


    public override IEnumerable<CodeMemberMethod> GetServerMappingServiceMethods()
    {
        foreach (var method in base.GetServerMappingServiceMethods())
        {
            yield return method;
        }

        if (this.HasMapToDomainObjectMethod)
        {
            foreach (var masterType in this.Configuration.GetDomainTypeMasters(this.DomainType, this.FileType, true))
            {
                if (this.Configuration.IsPersistentObject(masterType))
                {
                    yield return this.GetMappingServiceToDomainObjectMethod(masterType);
                }
            }
        }
    }

    protected override CodeExpression GetFieldInitExpression(CodeTypeReference codeTypeReference, PropertyInfo property)
    {
        if (!this.CodeTypeReferenceService.IsOptional(property))
        {
            if (property.PropertyType.IsCollection())
            {
                return this.CodeTypeReferenceService.GetCodeTypeReference(property, true).ToObjectCreateExpression();
            }
        }

        return base.GetFieldInitExpression(codeTypeReference, property);
    }
}
