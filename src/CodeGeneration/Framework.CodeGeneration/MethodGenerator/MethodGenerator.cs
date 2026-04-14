using System.CodeDom;
using System.Collections.ObjectModel;

using CommonFramework;

using Framework.CodeDom.Extensions;

namespace Framework.CodeGeneration.MethodGenerator;

public abstract class MethodGenerator : IMethodGenerator, IMethodGeneratorInfo
{
    private static readonly CodeMemberMethod DefaultMethod = new();


    private readonly Lazy<ReadOnlyCollection<CodeParameterDeclarationExpression>> lazyParameters;


    protected MethodGenerator() => this.lazyParameters = LazyHelper.Create(() => this.GetParameters().ToReadOnlyCollection());

    protected virtual MemberAttributes Attributes => DefaultMethod.Attributes;

    public abstract string Name { get; }

    public abstract CodeTypeReference ReturnType { get; }

    public ReadOnlyCollection<CodeParameterDeclarationExpression> Parameters => this.lazyParameters.Value;

    protected abstract IEnumerable<CodeParameterDeclarationExpression> GetParameters();

    protected abstract IEnumerable<CodeStatement> GetStatements();


    public CodeMemberMethod GetMethod() =>
        new CodeMemberMethod
            {
                Attributes = this.Attributes,

                Name = this.Name,

                ReturnType = this.ReturnType,
            }.WithParameters(this.Parameters)
             .WithStatements(this.GetStatements());

    IEnumerable<CodeParameterDeclarationExpression> IMethodGeneratorInfo.Parameters => this.Parameters;
}
