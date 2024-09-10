using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class ServerPrimitiveDTOMappingServiceFileFactory<TConfiguration> : FileFactory<TConfiguration, FileType>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public ServerPrimitiveDTOMappingServiceFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {
        this.BaseReference = this.Configuration.GetCodeTypeReference(null, ServerFileType.ServerPrimitiveDTOMappingServiceBase);
    }


    public override FileType FileType { get; } = ServerFileType.ServerPrimitiveDTOMappingService;


    public override CodeTypeReference BaseReference { get; }


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       TypeAttributes = TypeAttributes.Public,
                       IsPartial = true,
               };
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        var contextParameter = this.Configuration.BLLContextTypeReference.ToParameterDeclarationExpression("context");

        yield return new CodeConstructor
                     {
                             Attributes = MemberAttributes.Public,
                             Parameters = {contextParameter},
                             BaseConstructorArgs = {contextParameter.ToVariableReferenceExpression()}
                     };
    }
}
