using System.CodeDom;
using System.Reflection;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Mapping;

public class ServerPrimitiveDTOMappingServiceFileFactory<TConfiguration> : FileFactory<TConfiguration, BaseFileType>
        where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    public ServerPrimitiveDTOMappingServiceFileFactory(TConfiguration configuration)
            : base(configuration, null) =>
        this.BaseReference = this.Configuration.GetCodeTypeReference(null, ServerFileType.ServerPrimitiveDTOMappingServiceBase);

    public override BaseFileType FileType { get; } = ServerFileType.ServerPrimitiveDTOMappingService;


    public override CodeTypeReference BaseReference { get; }


    protected override CodeTypeDeclaration GetCodeTypeDeclaration() =>
        new(this.Name)
        {
            TypeAttributes = TypeAttributes.Public,
            IsPartial = true,
        };

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
