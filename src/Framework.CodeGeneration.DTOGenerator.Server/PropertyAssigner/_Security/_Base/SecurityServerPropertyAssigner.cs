using System.CodeDom;

using Framework.CodeDom;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner._Security.Base;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory._Helpers;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner.__Base;

namespace Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner._Security._Base;

public abstract class SecurityServerPropertyAssigner<TConfiguration>(IPropertyAssigner<TConfiguration> innerAssigner)
    : MaybePropertyAssigner<TConfiguration>(innerAssigner), IServerPropertyAssigner
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public CodeExpression MappingServiceRefExpr => new CodeThisReferenceExpression();

    public CodeExpression ContextRef => this.MappingServiceRefExpr.ToPropertyReference("Context");

    public CodeParameterDeclarationExpression DomainParameter => this.DomainType.GetDomainObjectParameter();
}
