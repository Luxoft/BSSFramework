using System.CodeDom;
using System.Reflection;
using Framework.CodeGeneration.DTOGenerator.FileFactory._Helpers;
using Framework.CodeGeneration.DTOGenerator.FileType;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.__Base.ByProperty;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory._Helpers;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Role._Base;

public abstract class RoleDTOFileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : DTOFileFactory<TConfiguration, DTOFileType>(configuration, domainType)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    protected abstract string DataContractNamespace { get; }


    protected override IPropertyAssigner MapDomainObjectToMappingObjectPropertyAssigner => new DomainObjectToDTOPropertyAssigner<TConfiguration>(this);


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Name = this.Name,
                       IsClass = true,
                       IsPartial = true,
                       TypeAttributes = TypeAttributes.Public
               };
    }

    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        yield return this.Configuration.GetDTOFileAttribute(this.DomainType, this.FileType);
        yield return this.GetDataContractCodeAttributeDeclaration(this.DataContractNamespace);
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        yield return this.GenerateDefaultConstructor();

        yield return this.GenerateFromDomainObjectConstructor(this.MapDomainObjectToMappingObjectPropertyAssigner);
    }
}
