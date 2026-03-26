using System.CodeDom;

using Framework.CodeDom;

namespace Framework.DomainDriven.DTOGenerator.Server;

public abstract class SecurityServerPropertyAssigner<TConfiguration> : MaybePropertyAssigner<TConfiguration>, IServerPropertyAssigner
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    protected SecurityServerPropertyAssigner(IPropertyAssigner<TConfiguration> innerAssigner)
            : base(innerAssigner)
    {
    }


    public CodeExpression MappingServiceRefExpr => new CodeThisReferenceExpression();

    public CodeExpression ContextRef => this.MappingServiceRefExpr.ToPropertyReference("Context");

    public CodeParameterDeclarationExpression DomainParameter => this.DomainType.GetDomainObjectParameter();
}
