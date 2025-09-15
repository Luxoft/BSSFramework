using System.CodeDom;
using System.Collections.ObjectModel;

using CommonFramework;

using Framework.CodeDom;
using Framework.Core;

namespace Framework.DomainDriven.Generation;

public abstract class MethodGenerator : IMethodGenerator, IMethodGeneratorInfo
{
    private static readonly CodeMemberMethod DefaultMethod = new CodeMemberMethod();


    private readonly Lazy<ReadOnlyCollection<CodeParameterDeclarationExpression>> _lazyParameters;


    protected MethodGenerator()
    {
        this._lazyParameters = LazyHelper.Create(() => this.GetParameters().ToReadOnlyCollection());
    }


    protected virtual MemberAttributes Attributes
    {
        get { return DefaultMethod.Attributes; }
    }

    public abstract string Name { get; }

    public abstract CodeTypeReference ReturnType { get; }

    public ReadOnlyCollection<CodeParameterDeclarationExpression> Parameters
    {
        get { return this._lazyParameters.Value; }
    }


    protected abstract IEnumerable<CodeParameterDeclarationExpression> GetParameters();

    protected abstract IEnumerable<CodeStatement> GetStatements();


    public CodeMemberMethod GetMethod()
    {
        return new CodeMemberMethod
               {
                       Attributes = this.Attributes,

                       Name = this.Name,

                       ReturnType = this.ReturnType,
               }.WithParameters(this.Parameters)
                .WithStatements(this.GetStatements());
    }



    IEnumerable<CodeParameterDeclarationExpression> IMethodGeneratorInfo.Parameters
    {
        get { return this.Parameters; }
    }
}
