using System.CodeDom;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.__Base.ByProperty;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory._Helpers;
using Framework.CodeGeneration.MethodGenerator;

namespace Framework.CodeGeneration.DTOGenerator.Server.Members.MapToDomainObject;

public class BaseMapToDomainObjectMethodFactory<TConfiguration, TFileFactory, TFileType> : IMethodGenerator
        where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
        where TFileFactory : DTOFileFactory<TConfiguration, TFileType>
        where TFileType : DTOFileType
{
    public readonly TFileFactory FileFactory;


    public readonly CodeParameterDeclarationExpression TargetDomainParameter;

    public readonly CodeVariableReferenceExpression TargetDomainParameterRefExpr;


    public readonly CodeParameterDeclarationExpression MappingServiceParameter;

    public readonly CodeVariableReferenceExpression MappingServiceParameterRefExpr;



    public BaseMapToDomainObjectMethodFactory(TFileFactory fileFactory)
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        this.FileFactory = fileFactory;

        this.TargetDomainParameter = this.FileFactory.GetDomainTypeTargetParameter();
        this.TargetDomainParameterRefExpr = this.TargetDomainParameter.ToVariableReferenceExpression();

        this.MappingServiceParameter = this.FileFactory.GetMappingServiceParameter();
        this.MappingServiceParameterRefExpr = this.MappingServiceParameter.ToVariableReferenceExpression();
    }


    public IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment> Configuration => this.FileFactory.Configuration;

    protected virtual MemberAttributes MemberAttributes { get; } = MemberAttributes.Public | MemberAttributes.Final;


    public CodeMemberMethod GetMethod() =>
        new()
        {
            Attributes = this.MemberAttributes,
            Name = this.Configuration.MapToDomainObjectMethodName,
            Parameters = { this.MappingServiceParameter, this.TargetDomainParameter },
            Statements = { this.GetMapMethodCodeStatements().Composite() }
        };

    protected virtual IEnumerable<CodeStatement> GetMapMethodCodeStatements()
    {
        yield return this.MappingServiceParameterRefExpr
                         .ToMethodInvokeExpression("Map" + this.FileFactory.DomainType.Name, new CodeThisReferenceExpression(), this.TargetDomainParameterRefExpr)
                         .ToExpressionStatement();
    }
}
