using System.CodeDom;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileFactory.Base;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory._Helpers;

namespace Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner.__Base;

public interface IServerPropertyAssigner : IPropertyAssigner
{
    CodeExpression MappingServiceRefExpr { get; }

    CodeExpression ContextRef { get; }

    CodeParameterDeclarationExpression DomainParameter { get; }
}


public abstract class ServerPropertyAssigner<TConfiguration>(IDTOSource<TConfiguration> source) : PropertyAssigner<TConfiguration>(source), IServerPropertyAssigner
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    public CodeExpression MappingServiceRefExpr => new CodeThisReferenceExpression();

    public CodeExpression ContextRef => this.MappingServiceRefExpr.ToPropertyReference("Context");

    public CodeParameterDeclarationExpression DomainParameter => this.DomainType.GetDomainObjectParameter();

}
