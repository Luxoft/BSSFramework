using System.CodeDom.Compiler;

namespace Framework.CodeDom.TypeScript
{
    public sealed class TypeScriptCodeDomProvider : CodeDomProvider
    {
        private ICodeGenerator generator;


        public TypeScriptCodeDomProvider(ICodeGenerator codeGenerator)
        {
            this.generator = codeGenerator;
        }


        public override string FileExtension { get; } = "ts";

        public override ICodeGenerator CreateGenerator()
        {
            return (ICodeGenerator)this.generator;
        }

        public override ICodeCompiler CreateCompiler()
        {
            return (ICodeCompiler)this.generator;
        }
    }
}
