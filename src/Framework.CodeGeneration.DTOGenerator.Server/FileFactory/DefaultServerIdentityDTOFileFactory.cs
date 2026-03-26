using System.CodeDom;

using Framework.CodeDom;
using Framework.CodeGeneration.DTOGenerator.FileFactory;
using Framework.CodeGeneration.DTOGenerator.FileFactory._Helpers;
using Framework.CodeGeneration.DTOGenerator.FileType;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.__Base.ByProperty;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory._Helpers;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory;

public class DefaultServerIdentityDTOFileFactory<TConfiguration>(TConfiguration configuration, Type domainType)
    : DefaultIdentityDTOFileFactory<TConfiguration>(configuration, domainType), IDTOFileFactory<TConfiguration, DTOFileType>
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public IEnumerable<CodeMemberMethod> GetServerMappingServiceInterfaceMethods()
    {
        yield return this.GetMappingServiceInterfaceToDomainObjectMethod();
    }

    public IEnumerable<CodeMemberMethod> GetServerMappingServiceMethods()
    {
        yield return this.GetMappingServiceToDomainObjectMethod();
    }


    protected override IEnumerable<CodeConstructor> GetConstructors()
    {
        foreach (var baseCtor in base.GetConstructors())
        {
            yield return baseCtor;
        }

        {
            var domainParameter = this.GetDomainObjectParameter();

            yield return new CodeConstructor
                         {
                                 Attributes = MemberAttributes.Public,
                                 Parameters = { domainParameter },
                                 Statements =
                                 {
                                         new CodeThrowArgumentNullExceptionConditionStatement(domainParameter),
                                         domainParameter.ToVariableReferenceExpression().ToPropertyReference(this.Configuration.Environment.IdentityProperty)
                                                        .ToAssignStatement(this.GetAssignIdExpression())
                                 }
                         };
        }

        if (this.Configuration.IdentityIsReference)
        {
            yield return this.GenerateDefaultConstructor();
        }

        {
            var idParameter = new CodeParameterDeclarationExpression(typeof(string), "id");

            yield return new CodeConstructor
                         {
                                 Attributes = MemberAttributes.Public,
                                 Parameters = { idParameter },
                                 ChainedConstructorArgs = { typeof(Guid).ToTypeReference().ToObjectCreateExpression(idParameter.ToVariableReferenceExpression())}
                         };
        }
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }

        yield return this.GetToDomainObjectMethod();
    }

    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        foreach (var customAttribute in base.GetCustomAttributes())
        {
            yield return customAttribute;
        }

        yield return this.Configuration.GetDTOFileAttribute(this.DomainType, this.FileType);
    }
}
