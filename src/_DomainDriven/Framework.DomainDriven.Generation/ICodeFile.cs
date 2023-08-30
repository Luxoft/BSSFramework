using System.CodeDom;

using Framework.CodeDom;

namespace Framework.DomainDriven.Generation;

public interface ICodeFile : IRenderingFile<CodeNamespace>
{

}

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
        private readonly ICodeFile _baseCodeFile;

        private readonly CodeDomVisitor _visitor;


        public VisitedCodeFile(ICodeFile baseCodeFile, CodeDomVisitor visitor)
        {
            if (baseCodeFile == null) throw new ArgumentNullException(nameof(baseCodeFile));
            if (visitor == null) throw new ArgumentNullException(nameof(visitor));

            this._baseCodeFile = baseCodeFile;
            this._visitor = visitor;
        }


        public string Filename
        {
            get { return this._baseCodeFile.Filename; }
        }


        public CodeNamespace GetRenderData()
        {
            return this._visitor.VisitNamespace(this._baseCodeFile.GetRenderData());
        }
    }
}
