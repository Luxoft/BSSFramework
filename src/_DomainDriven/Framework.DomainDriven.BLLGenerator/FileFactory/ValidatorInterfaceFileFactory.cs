using System.CodeDom;

using Framework.CodeDom;
using Framework.Validation;

namespace Framework.DomainDriven.BLLGenerator;

public class ValidatorInterfaceFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public ValidatorInterfaceFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {
    }

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
