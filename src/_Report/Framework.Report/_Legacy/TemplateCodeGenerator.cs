using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;

namespace ExcelTemplate
{
    public class TemplateCodeGenerator
    {
        //private readonly string evaluatorNamespace = "ExpressionEvaluation";
        //private readonly string evaluatorClassName = "Evaluator";
        //private readonly string evaluatorMethodName = "Evaluate";
        //private string name;
        //private IList<TemplateVariable> variables = new List<TemplateVariable>();

        //public IList<TemplateVariable> Variables
        //{
        //    get { return variables; }
        //}

        //private string GenerateEvaluatorSource(string expression)
        //{
        //    StringBuilder source = new StringBuilder();
        //    source.AppendLine("using System;");
        //    source.AppendLine("using System.Collections.Generic;");
        //    source.AppendLine("using System.Text;");
        //    source.AppendLine("namespace " + evaluatorNamespace);
        //    source.AppendLine("{");
        //    source.AppendLine("public class " + evaluatorClassName);
        //    source.AppendLine("{");
        //    source.AppendLine("public static string " + evaluatorMethodName + "(");
        //    string comma = "";
        //    foreach (TemplateVariable var in variables)
        //    {
        //        source.Append(comma);
        //        comma = ",";
        //        source.Append(var.Type.FullName);
        //        source.Append(" ");
        //        source.Append(var.Name);
        //    }
        //    source.Append(")");
        //    source.AppendLine("{");
        //    source.Append("return ");
        //    source.Append(expression);
        //    source.AppendLine(".ToString();");
        //    source.AppendLine("}");
        //    source.AppendLine("}");
        //    source.AppendLine("}");
        //    return source.ToString();
        //}
        //private Assembly CompileSource(string source)
        //{
        //    CompilerParameters compilerParams = new CompilerParameters(new string[] { "system.dll" });
        //    CodeDomProvider provider = Microsoft.CSharp.CSharpCodeProvider.CreateProvider("C#");
        //    CompilerResults result = provider.CompileAssemblyFromSource(compilerParams, source.ToString());
        //    return result.CompiledAssembly;
        //}

        //public string EvaluateExpression(string expression)
        //{
        //    Assembly expressionAssembly = CompileSource(GenerateEvaluatorSource(expression));
        //    Type evaluatorType = expressionAssembly.GetType(evaluatorNamespace + "." + evaluatorClassName);
        //    return (string)evaluatorType.InvokeMember(evaluatorMethodName, BindingFlags.Public | BindingFlags.Static, null, null, new object[] { expression });
        //}
    }
}
