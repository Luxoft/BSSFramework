using System.CodeDom;

using Framework.Core;

using Microsoft.CSharp;

namespace Framework.CodeDom;

public abstract class CodeDomRenderer :
        IFileRenderer<CodeExpression, string>,
        IFileRenderer<CodeNamespace, string>,
        IFileRenderer<CodeCompileUnit, string>,
        IFileRenderer<CodeStatement, string>
{
    internal readonly IDictionaryCache<CodeBinaryOperatorType, string> BinaryOperators;


    protected CodeDomRenderer()
    {
        this.BinaryOperators = new DictionaryCache<CodeBinaryOperatorType, string>(this.Render).WithLock();
    }


    public abstract string FileExtension { get; }


    public abstract string Render(CodeExpression codeExpression);

    public abstract string Render(CodeNamespace codeNamespace);

    public abstract string Render(CodeCompileUnit compileUnit);

    public abstract string Render(CodeStatement codeStatement);


    protected abstract string Render(CodeBinaryOperatorType @operator);

    public static readonly CodeDomRenderer CSharp = new CSharpCodeDomRenderer(new CSharpCodeProvider(new Dictionary<string, string>
                                                                                  {
                                                                                          {"CompilerVersion", "v4.0"}
                                                                                  }));
}
