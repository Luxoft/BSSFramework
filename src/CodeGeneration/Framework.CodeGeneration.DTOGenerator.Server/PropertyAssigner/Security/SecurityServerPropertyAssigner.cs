using System.CodeDom;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner.Security;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory;

namespace Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner.Security;

public abstract class SecurityServerPropertyAssigner<TConfiguration>(IPropertyAssigner<TConfiguration> innerAssigner)
    : MaybePropertyAssigner<TConfiguration>(innerAssigner), IServerPropertyAssigner
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    public CodeExpression MappingServiceRefExpr => new CodeThisReferenceExpression();

    public CodeExpression ContextRef => this.MappingServiceRefExpr.ToPropertyReference("Context");

    public CodeParameterDeclarationExpression DomainParameter => this.DomainType.GetDomainObjectParameter();
}
