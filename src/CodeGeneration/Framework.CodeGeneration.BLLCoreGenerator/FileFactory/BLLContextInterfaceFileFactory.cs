using System.CodeDom;

using Framework.BLL;
using Framework.BLL.Services;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLCoreGenerator.Configuration;
using Framework.FileGeneration.Configuration;

namespace Framework.CodeGeneration.BLLCoreGenerator.FileFactory;

public class BLLContextInterfaceFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IBLLCoreGeneratorConfiguration<IBLLCoreGenerationEnvironment>
{
    public override FileType FileType => FileType.BLLContextInterface;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration() =>
        new()
        {
            Name = this.Name,
            Attributes = MemberAttributes.Public,
            IsPartial = true,
            IsInterface = true,
        };

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield return typeof(ISecurityBLLContext<,>).MakeGenericType(
            this.Configuration.Environment.PersistentDomainObjectBaseType,
            this.Configuration.Environment.GetIdentityType()).ToTypeReference();

        yield return typeof(IBLLFactoryContainerContext<>).ToTypeReference(this.Configuration.GetCodeTypeReference(null, FileType.BLLFactoryContainerInterface));
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        yield return new CodeMemberProperty
        {
            Name = "Logics",
            Attributes = MemberAttributes.New,
            Type = this.Configuration.GetCodeTypeReference(null, FileType.BLLFactoryContainerInterface),
            HasGet = true
        };
    }
}
