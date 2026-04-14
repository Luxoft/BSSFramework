using System.Collections.Concurrent;
using System.Linq.Expressions;

using CommonFramework.ExpressionEvaluate;

using Framework.Core;
using Framework.ExpressionParsers.CSharp;
using Framework.ExpressionParsers.Native;

namespace Framework.ExpressionParsers;

public class ExpressionParser<TDelegate>(INativeExpressionParser parser, ILambdaCompileCache lambdaCompileCache)
    : ExpressionParser<TDelegate, Expression<TDelegate>>
    where TDelegate : Delegate
{
    public ExpressionParser(INativeExpressionParser parser)
        : this(parser, new LambdaCompileCache(LambdaCompileMode.None))
    {
    }

    public ExpressionParser()
        : this(CSharpNativeExpressionParser.Compile)
    {
    }

    protected override Expression<TDelegate> GetInternalExpression(string value) => parser.Parse<TDelegate>(value);

    protected override TDelegate CompileExpression(Expression<TDelegate> expression) => lambdaCompileCache.GetFunc(expression);
}

public abstract class ExpressionParser<TDelegate, TExpression> : IExpressionParser<string, TDelegate, TExpression>
    where TDelegate : Delegate
    where TExpression : LambdaExpression
{
    private readonly ConcurrentDictionary<string, TExpression> expressionCache = [];

    private readonly ConcurrentDictionary<string, TDelegate> delegateCache = [];

    public Type ExpressionType => typeof(TDelegate);

    public virtual string ExpectedFormat => $"\"{this.ExpressionType.ToCSharpShortName()}\"";

    protected abstract TExpression GetInternalExpression(string value);

    protected abstract TDelegate CompileExpression(TExpression expression);

    public TExpression GetExpression(string value) =>
        this.expressionCache.GetOrAdd(
            value,
            _ =>
            {
                try
                {
                    return this.GetInternalExpression(value);
                }
                catch (Exception ex)
                {
                    throw new ExpressionParingException($"Can't parse value: \"{value}\". Expected format: {this.ExpectedFormat}", ex);
                }
            });

    public TDelegate GetDelegate(string value) =>
        this.delegateCache.GetOrAdd(value, _ => this.CompileExpression(this.GetExpression(value)));

    public virtual void Validate(string source) => this.GetExpression(source);
}
