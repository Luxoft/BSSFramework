using System.CodeDom;
using System.Runtime.Serialization;

using Framework.CodeDom;
using Framework.CodeGeneration.DomainMetadata;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileFactory._Helpers;
using Framework.CodeGeneration.DTOGenerator.FileType;

using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main.Base;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main;

public class DefaultBasePersistentDTOFileFactory<TConfiguration>(TConfiguration configuration)
    : MainDTOFileFactory<TConfiguration>(configuration, configuration.Environment.PersistentDomainObjectBaseType)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public override MainDTOFileType FileType { get; } = BaseFileType.BasePersistentDTO;


    protected override bool ConvertToStrict { get; } = false;


    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        foreach (var baseType in base.GetBaseTypes())
        {
            yield return baseType;
        }

        yield return this.GetIdentityObjectTypeRef();
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers())
        {
            yield return baseMember;
        }


        yield return new CodeMemberProperty
                     {
                             Attributes = MemberAttributes.Public | MemberAttributes.Final,
                             Name = "IsNew",
                             Type = new CodeTypeReference(typeof(bool)),
                             GetStatements =
                             {
                                     new CodeValueEqualityOperatorExpression(
                                                                             this.Configuration.Environment.GetIdentityType().ToTypeReference().ToDefaultValueExpression(),
                                                                             this.Configuration.GetIdentityPropertyCodeExpression()).ToMethodReturnStatement()
                             },
                             CustomAttributes =
                             {
                                 new CodeAttributeDeclaration(new CodeTypeReference(typeof(IgnoreDataMemberAttribute))),
                             }
                     };

        yield return this.GetIdentityObjectImplementation(true);
    }
}
