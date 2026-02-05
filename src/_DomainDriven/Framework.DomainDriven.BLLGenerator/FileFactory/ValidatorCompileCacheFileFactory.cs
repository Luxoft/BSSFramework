using System.CodeDom;

using Framework.CodeDom;
using Framework.Validation;

namespace Framework.DomainDriven.BLLGenerator;

public class ValidatorCompileCacheFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public ValidatorCompileCacheFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {
    }

    public override FileType FileType => FileType.ValidatorCompileCache;

    public override CodeTypeReference BaseReference { get; } = typeof(ValidatorCompileCache).ToTypeReference();

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Name = this.Name,
                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
               };
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        var validationMapParam =
                this.Configuration.GetCodeTypeReference(this.DomainType, FileType.ValidationMap)
                    .ToParameterDeclarationExpression("validationMap");

        yield return new CodeConstructor
                     {
                         Attributes = MemberAttributes.Public,
                         Parameters = { validationMapParam },
                         BaseConstructorArgs = { validationMapParam.ToVariableReferenceExpression() }
                     };
    }
}
