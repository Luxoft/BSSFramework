using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using JetBrains.Annotations;

namespace Framework.ExpressionParsers;

public class CSharpNativeExpressionParser : INativeExpressionParser
{
    private readonly INativeBodyExpressionParser _parser;


    public CSharpNativeExpressionParser(INativeBodyExpressionParser parser)
    {
        if (parser == null) throw new ArgumentNullException(nameof(parser));

        this._parser = parser;
    }



    public LambdaExpression Parse([NotNull] NativeExpressionParsingData input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));

        var funcMethodInfo = input.ParsingInfo;

        var expression = input.ParsingValue;

        var expressionBody = expression;
        var parameterNames = this.GetParametersNames(expression).ToList();
        var expressionParameters = this.GetInputFuncParameters(parameterNames, funcMethodInfo).ToArray();
        var separateStartIndex = expression.IndexOf("=>");
        if (-1 != separateStartIndex)
        {
            expressionBody = new string(expression.Skip(separateStartIndex + 2).ToArray());
        }
        return this.ParseExpression(expressionParameters, funcMethodInfo.ReturnType, expressionBody);
    }

    private IEnumerable<ParameterExpression> GetInputFuncParameters(IEnumerable<string> parameterNames, MethodTypeInfo methodInfo)
    {
        var genericParameterSequencies =
                methodInfo
                        .InputTypes
                        .Select((parameterType, index) => new {ParameterType = parameterType, Index = index})
                        .ToList();

        var expressionParameterNames =
                parameterNames
                        .Select((name, index) => new {Name = name, Index = index})
                        .ToList();

        if (!expressionParameterNames.Any())
        {
            expressionParameterNames =
                    genericParameterSequencies.Select((_, index) => new {Name = string.Empty, Index = index}).ToList();
        }


        if (genericParameterSequencies.Count != expressionParameterNames.Count)
        {
            throw new ArgumentException("Amount generic parameters not equal input parameters in expression");
        }

        return genericParameterSequencies.Join(expressionParameterNames, z => z.Index, z => z.Index,
                                               (parameter, name) =>
                                                       Expression.Parameter(parameter.ParameterType, name.Name));

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


    private LambdaExpression ParseExpression(ParameterExpression[] parameters, Type returnType, string expression)
    {
        return (LambdaExpression)this._parser.Parse(parameters, returnType, expression);
    }



    /// <summary>
    /// Parse only primitive types
    /// </summary>
    /// <returns></returns>
    public static readonly CSharpNativeExpressionParser Default = new CSharpNativeExpressionParser(new MicrosoftCSharpExpressionParser());

    /// <summary>
    /// Parse via compile
    /// </summary>
    /// <returns></returns>
    public static readonly CSharpNativeExpressionParser Compile = new CSharpNativeExpressionParser(new RoslynCSharpExpressionParser());


    public static readonly INativeExpressionParser Composite = new INativeExpressionParser[]
                                                               {
                                                                       CSharpNativeExpressionParser.Default,
                                                                       CSharpNativeExpressionParser.Compile
                                                               }.ToSafeComposite().WithCache();
}
