using System.CodeDom.Compiler;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Microsoft.CSharp;

using Framework.Core;

namespace Framework.ExpressionParsers;

/// <summary>
/// TODO: replace to ScriptEngine by roslyn
/// </summary>
public class ScriptEngineService
{
    public static ScriptEngineService Value = new ScriptEngineService();

    private ScriptEngineService()
    {

    }

    public T Execute<T>(string script, params Type[] typeParameters)
    {
        var types = typeParameters
                    .SelectMany(z => z.IsGenericType ? z.GetGenericArguments().Concat(new[] { z }) : new[] { z })
                    .Distinct()
                    .ToList();

        if (new Type[] { typeof(Func<,>), typeof(Func<,,>) }.All(z => !z.IsAssignableFrom(typeof(T).GetGenericTypeDefinition())))
        {
            throw new NotImplementedException();
        }

        var inputTypes = typeof(T).GetGenericArguments().SkipLast(1).ToArray();
        var resultType = typeof(T).GetGenericArguments().Last();

        var expressionBody = script;

        var parameterNames = this.GetParametersNames(script).ToList();
        var expressionParameters = this.GetInputFuncParameters(parameterNames, new MethodTypeInfo(inputTypes, resultType)).ToArray();
        var separateStartIndex = script.IndexOf("=>");

        if (-1 != separateStartIndex)
        {
            expressionBody = new string(script.Skip(separateStartIndex + 2).ToArray());
        }


        var assembliNames =
                types
                        .SelectMany(z => z.Assembly.GetReferencedAssemblies().Concat(new[] { z.Assembly.GetName() })).Distinct().ToArray();

        var assemblyPaths = assembliNames.Select(Assembly.Load).ToList();


        var usingNamespaces =
                string.Join(Environment.NewLine,
                            assemblyPaths
                                    .SelectMany(z => z.GetTypes())
                                    .Where(z => z.IsPublic)
                                    .Select(q => q.Namespace)
                                    .Where(z => !string.IsNullOrWhiteSpace(z))
                                    .Select(q => $"using {q};")
                                    .Distinct()
                                    .ToList());



        var assembliesFullNames = assemblyPaths.Select(z => z.Location).ToArray();

        var compileParameters = new CompilerParameters(assembliesFullNames) { GenerateInMemory = true };

        var provider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", $"v{Environment.Version.Major}.0" } });// + Environment.Version.ToString()}});



        var methodType =
                $"{this.GetGenericTypeName(typeof(Func<>))}<{string.Join(",", expressionParameters.Select(z => z.Type).Concat(new[] { resultType }).Select(t => t.ToCSharpFullName()))}>";

        var methodName = "Execute";
        var typeName = "ExpressionParser";
        var namespaceName = "Generated";


        var source = usingNamespaces + Environment.NewLine +
                     "namespace " + namespaceName + Environment.NewLine +
                     "{" +
                     "  public static class " + typeName + Environment.NewLine +
                     "  {" +
                     "      public static  " + methodType + " " + methodName + "()" + Environment.NewLine +
                     "      {" +
                     " return " + $"({string.Join(",", expressionParameters.Select(z => z.Name))})=>{expressionBody}" +
                     "}" + Environment.NewLine + "  }"
                     + Environment.NewLine +
                     "}" + Environment.NewLine +
                     "" + Environment.NewLine +
                     "";


        var compileResult = provider.CompileAssemblyFromSource(compileParameters, source);

        if (0 != compileResult.Errors.Count)
        {
            var errorTextBuilder = compileResult.Errors.Cast<CompilerError>().Aggregate(new StringBuilder(), (builder, error) => builder.Append(error.ErrorNumber).Self(z => z.AppendLine(error.ErrorText)));
            var fullError = new[] { errorTextBuilder.ToString(), source }.Join(Environment.NewLine);
            var exception = new ParseException("Ошибки компиляции. \n" + fullError, 0);
            throw exception;
        }
        var type = compileResult.CompiledAssembly.GetType($"{namespaceName}.{typeName}");

        var method = type.GetMethod(methodName);
        var result = (T)method.Invoke(null, new object[0]);
        return result;
    }

    private IEnumerable<ParameterExpression> GetInputFuncParameters(IEnumerable<string> parameterNames, MethodTypeInfo methodInfo)
    {
        var genericParameterSequencies =
                methodInfo
                        .InputTypes
                        .Select((parameterType, index) => new { ParameterType = parameterType, Index = index })
                        .ToList();

        var expressionParameterNames =
                parameterNames
                        .Select((name, index) => new { Name = name, Index = index })
                        .ToList();

        if (!expressionParameterNames.Any())
        {
            expressionParameterNames =
                    genericParameterSequencies.Select((_, index) => new { Name = string.Empty, Index = index }).ToList();
        }


        if (genericParameterSequencies.Count != expressionParameterNames.Count)
        {
            throw new System.ArgumentException("Amount generic parameters not equal input parameters in expression");
        }

        return genericParameterSequencies.Join(expressionParameterNames, z => z.Index, z => z.Index,
                                               (parameter, name) =>
                                                       Expression.Parameter(parameter.ParameterType, name.Name));

    }

    private string GetGenericTypeName(Type source)
    {
        return $"{source.Namespace}.{new string(source.Name.SkipLast(2).ToArray())}";
    }

    private IEnumerable<string> GetParametersNames(string expression)
    {
        if (!expression.Contains("=>"))
        {
            yield break;
        }

        var currentResult = new List<char>();
        Func<string> returnFunc = () =>
                                  {
                                      var result = new string(currentResult.ToArray());
                                      currentResult.Clear();
                                      return result;
                                  };

        foreach (var currentChar in expression.TakeWhile(z => z != '='))
        {
            if (currentChar == '(')
            {
                continue;
            }
            if (currentChar == ')' && currentResult.Any())
            {
                yield return returnFunc();
                continue;
            }
            if (currentChar == ' ')
            {
                continue;
            }
            if (currentChar == ',')
            {
                if (!currentResult.Any())
                {
                    var parametersString = new string(expression.TakeWhile(z => z != '=').ToArray());

                    throw new ParseException(
                                             $"Parameters has no correct format:{parametersString}", 0);
                }
                yield return returnFunc();
                continue;
            }
            currentResult.Add(currentChar);
        }
        if (currentResult.Any())
        {
            yield return returnFunc();
        }
    }
}
