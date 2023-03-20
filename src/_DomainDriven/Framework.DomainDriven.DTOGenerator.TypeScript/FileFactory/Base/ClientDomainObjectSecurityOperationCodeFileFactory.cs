using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base;

/// <summary>
/// Client dependency security operation code file factory
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class ClientDomainObjectSecurityOperationCodeFileFactory<TConfiguration> : DomainObjectSecurityOperationCodeFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public ClientDomainObjectSecurityOperationCodeFileFactory(
            TConfiguration configuration, Type domainType, IEnumerable<Enum> securityOperations)
            : base(configuration, domainType, securityOperations)
    {
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var field in this.SecurityOperations)
        {
            yield return new CodeMemberField
                         {
                                 Name = field.ToString(),
                                 InitExpression = new CodePrimitiveExpression(Convert.ChangeType(field, field.GetTypeCode())),
                                 CustomAttributes =
                                 {
                                         new CodeAttributeDeclaration(new CodeTypeReference(typeof(EnumMemberAttribute)))
                                 },
                         };
        }
    }
}
