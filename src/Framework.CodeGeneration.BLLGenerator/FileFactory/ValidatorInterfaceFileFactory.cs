using System.CodeDom;

using Framework.CodeDom;
using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.CodeGeneration.BLLGenerator.FileFactory.__Base;
using Framework.Validation;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory;

public class ValidatorInterfaceFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override FileType FileType => FileType.ValidatorInterface;

    public override CodeTypeReference BaseReference { get; } = typeof(IValidator).ToTypeReference();

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                   Name = this.Name,
                   Attributes = MemberAttributes.Public,
                   IsPartial = true,
                   IsInterface = true
        };
    }
}
