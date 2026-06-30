using System.CodeDom;

using Framework.CodeDom;

namespace Framework.CodeGeneration.Extensions;

public static class CodeFileExtensions
{
    public static ICodeFile WithVisitor(this ICodeFile codeFile, CodeDomVisitor visitor)
    {
        if (codeFile is null) throw new ArgumentNullException(nameof(codeFile));
        if (visitor is null) throw new ArgumentNullException(nameof(visitor));

        return new VisitedCodeFile(codeFile, visitor);
    }


    private class VisitedCodeFile(ICodeFile baseCodeFile, CodeDomVisitor visitor) : ICodeFile
    {
        private readonly ICodeFile baseCodeFile = baseCodeFile ?? throw new ArgumentNullException(nameof(baseCodeFile));

        private readonly CodeDomVisitor visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));

        public string Filename => this.baseCodeFile.Filename;

        public CodeNamespace GetRenderData() => this.visitor.VisitNamespace(this.baseCodeFile.GetRenderData());
    }
}

