using System.Linq.Expressions;
using System.Runtime.Loader;

using CommonFramework;

using Framework.Core;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Framework.ExpressionParsers;

public class RoslynCSharpExpressionParser : INativeBodyExpressionParser
{
    public RoslynCSharpExpressionParser()
    {
    }


    public Expression Parse(ParameterExpression[] parameters, Type returnType, string expression)
    {
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));
        if (returnType == null) throw new ArgumentNullException(nameof(returnType));
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        var types = parameters.Select(parameter => parameter.Type).Concat(
                                                                          new[] { returnType }).Concat(new[] { typeof(Enumerable), typeof(EnumerableExtensions), typeof(Expression), typeof(object) })
                              .GetCompileReferencedTypes()
                              .ToList();

        var assemblies = types.Select(t => t.Assembly)
                              .GetCompileReferencedAssemblies()
                              .Where(a => !a.IsDynamic)
                              .ToList();

        var usingNamespaces = assemblies.SelectMany(z => z.GetExportedTypes().Select(q => q.Namespace))
                                        .Distinct()
                                        .Where(z => !string.IsNullOrWhiteSpace(z))
                                        .Select(q => $"using {q};")
                                        .Join(Environment.NewLine);

        var assembliesFullNames = assemblies.Select(z => z.Location).ToArray();

        var methodType = typeof(Expression<>)
                         .MakeGenericType(parameters.Select(parameter => parameter.Type).ToDelegateType(returnType))
                         .ToCSharpFullName();

        var methodName = "GetExpression";
        var typeName = nameof(ExpressionParser);
        var namespaceName = "Generated";


        var source = $"{usingNamespaces}namespace {namespaceName}{{  public static class {typeName}  {{      public static  {methodType} {methodName}()      {{          return ({parameters.Join(", ", arg => arg.Name)}) => {expression};      }}  }}}}";


        //var dotnetCoreDirectory = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();

        var references = new MetadataReference[]
                         {
                                 //MetadataReference.CreateFromFile(Path.Combine(dotnetCoreDirectory, "mscorlib.dll")),
                                 //MetadataReference.CreateFromFile(Path.Combine(dotnetCoreDirectory, "netstandard.dll")),
                                 //MetadataReference.CreateFromFile(Path.Combine(dotnetCoreDirectory, "System.Runtime.dll"))
                         }.Concat(assembliesFullNames.Select(f => MetadataReference.CreateFromFile(f)));

        var compilation = CSharpCompilation.Create(Guid.NewGuid().ToString())
                                           .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                                           .AddReferences(references)
                                           .AddSyntaxTrees(CSharpSyntaxTree.ParseText(source));

        using (var stream = new MemoryStream())
        {
            var compileResult = compilation.Emit(stream);

            if (compileResult.Success)
            {
                stream.Position = 0;

                var assembly = AssemblyLoadContext.Default.LoadFromStream(stream);

                var expressionParserType = assembly.GetType($"{namespaceName}.{typeName}");

                return expressionParserType.GetMethod(methodName).Invoke<Expression>(null);
            }
            else
            {

                var errorTextBuilder = compileResult.Diagnostics.Join(",", d => $"{d.Id}: {d.GetMessage()}");
                var exception = new ParseException(errorTextBuilder, 0);
                throw exception;
            }
        }
    }
}
