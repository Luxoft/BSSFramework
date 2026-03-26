using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileType;

using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory._Helpers;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main.Base;
using Framework.Core;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main;

public class DefaultRichDTOFileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : MainDTOFileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public override MainDTOFileType FileType { get; } = BaseFileType.RichDTO;


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
