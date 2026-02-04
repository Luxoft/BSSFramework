using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.ServiceModelGenerator;
using Framework.DomainDriven.ServiceModelGenerator.Configuration._Base;
using Microsoft.AspNetCore.Mvc;

namespace Framework.DomainDriven.WebApiGenerator.NetCore.SingleController;

public class SingleControllerCodeFileFactory<TConfiguration> : ImplementFileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public SingleControllerCodeFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    public override FileType FileType { get; } = FileType.Implement;

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield break;
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var baseMember in base.GetMembers().Select(CodeDomVisitor.Identity.VisitTypeMember))
        {
            if (baseMember is CodeMemberMethod method)
            {
                if (method.Attributes.HasFlag(MemberAttributes.Public))
                {
                    if (this.Configuration.UseRouteAction)
                    {
                        method.CustomAttributes.Add(typeof(HttpPostAttribute).ToTypeReference().ToAttributeDeclaration());
                    }
                    else
                    {
                        method.CustomAttributes.Add(typeof(HttpPostAttribute).ToTypeReference().ToAttributeDeclaration(new CodeNameofExpression(method.Name).ToAttributeArgument()));
                    }

                    foreach (CodeParameterDeclarationExpression parameter in method.Parameters)
                    {
                        parameter.CustomAttributes.Add(typeof(FromFormAttribute).ToTypeReference().ToAttributeDeclaration());
                    }
                }

                yield return baseMember;
            }
        }
    }
}
