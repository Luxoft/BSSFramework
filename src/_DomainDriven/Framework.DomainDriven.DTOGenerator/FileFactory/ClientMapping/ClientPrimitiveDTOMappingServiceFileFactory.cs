using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;

namespace Framework.DomainDriven.DTOGenerator;

public class ClientPrimitiveDTOMappingServiceFileFactory<TConfiguration> : FileFactory<TConfiguration, FileType>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public ClientPrimitiveDTOMappingServiceFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {
        this.BaseReference = this.Configuration.GetCodeTypeReference(null, FileType.ClientPrimitiveDTOMappingServiceBase);
    }


    public override FileType FileType { get; } = FileType.ClientPrimitiveDTOMappingService;

    public override CodeTypeReference BaseReference { get; }

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       TypeAttributes = TypeAttributes.Public,
                       IsPartial = true
               };
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        var currentInstanceField = new CodeMemberField
                                   {
                                           Attributes = MemberAttributes.Private | MemberAttributes.Static,
                                           Name = "_default",
                                           Type = this.CurrentReference,
                                           InitExpression = this.CurrentReference.ToObjectCreateExpression(),
                                   };

        yield return currentInstanceField;

        yield return new CodeMemberProperty
                     {
                             Attributes = MemberAttributes.Public | MemberAttributes.Static,
                             Type = this.CurrentReference,
                             Name = "Default",
                             HasGet = true,
                             GetStatements =
                             {
                                     this.CurrentReference.ToTypeReferenceExpression()
                                         .ToFieldReference(currentInstanceField)
                                         .ToMethodReturnStatement()
                             }
                     };
    }
}
