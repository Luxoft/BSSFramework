using System.CodeDom;
using System.Reflection;
using System.Runtime.Serialization;

using Framework.SecuritySystem;

namespace Framework.DomainDriven.DTOGenerator;

public class DomainObjectSecurityRuleCodeFileFactory<TConfiguration> : FileFactory<TConfiguration, RoleFileType>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected readonly IReadOnlyCollection<SecurityRule> SecurityRules;


    public DomainObjectSecurityRuleCodeFileFactory(TConfiguration configuration, Type domainType, IEnumerable<SecurityRule> securityRules)
            : base(configuration, domainType)
    {
        if (securityRules == null) throw new ArgumentNullException(nameof(securityRules));

        this.SecurityRules = securityRules.ToArray();
    }


    public override RoleFileType FileType { get; } = DTOGenerator.FileType.DomainObjectSecurityRuleCode;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration(this.Name)
               {
                       TypeAttributes = TypeAttributes.Public,
                       IsEnum = true
               };
    }

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield break;
    }

    protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
    {
        yield return this.GetDataContractCodeAttributeDeclaration();
    }


    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        return from securityRule in this.SecurityRules

               select new CodeMemberField
                      {
                              Name = securityRule.ToString(),
                              CustomAttributes =
                              {
                                      new CodeAttributeDeclaration (new CodeTypeReference(typeof(EnumMemberAttribute)))
                              }
                      };
    }
}
