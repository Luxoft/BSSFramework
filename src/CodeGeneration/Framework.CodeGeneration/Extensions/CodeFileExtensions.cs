using System.CodeDom;

using Framework.CodeDom;

namespace Framework.CodeGeneration.Extensions;

public static class CodeFileExtensions
{
    public static ICodeFile WithVisitor(this ICodeFile codeFile, CodeDomVisitor visitor)
    {
        if (codeFile == null) throw new ArgumentNullException(nameof(codeFile));
        if (visitor == null) throw new ArgumentNullException(nameof(visitor));

        return new VisitedCodeFile(codeFile, visitor);
    }


    private class VisitedCodeFile : ICodeFile
    {
        private readonly ICodeFile baseCodeFile;

        private readonly CodeDomVisitor visitor;


        public VisitedCodeFile(ICodeFile baseCodeFile, CodeDomVisitor visitor)
        {
            if (baseCodeFile == null) throw new ArgumentNullException(nameof(baseCodeFile));
            if (visitor == null) throw new ArgumentNullException(nameof(visitor));

            this.baseCodeFile = baseCodeFile;
            this.visitor = visitor;
        }


        public string Filename
        {
            get { return this.baseCodeFile.Filename; }
        }


        public CodeNamespace GetRenderData()
        {
            return this.visitor.VisitNamespace(this.baseCodeFile.GetRenderData());
        }
    }
}
