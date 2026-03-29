using System.Linq.Expressions;

using CommonFramework.DictionaryCache;
using CommonFramework.ExpressionEvaluate;

using Framework.Core;

namespace Framework.ExpressionParsers;

public abstract class ExpressionParser<TDelegate>(INativeExpressionParser parser)
    : ExpressionParser<TDelegate, Expression<TDelegate>>(parser, expr => LambdaCompileCache.GetFunc(expr))
    where TDelegate : class
{
    protected override Expression<TDelegate> GetInternalExpression(string value) => this.Parser.Parse<TDelegate>(value);

    private static readonly ILambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.None);
}

public abstract class ExpressionParser<TDelegate, TExpression> : NativeExpressionParserContainer,

                                                                 IExpressionParser<string, TDelegate, TExpression>

        where TDelegate : class
        where TExpression : LambdaExpression
{
    private readonly IDictionaryCache<string, TExpression> expressionCache;

    private readonly IDictionaryCache<string, TDelegate> delegateCache;


    protected ExpressionParser(INativeExpressionParser parser, Func<TExpression, TDelegate> compileFunc)
            : base (parser)
    {
        if (parser == null) throw new ArgumentNullException(nameof(parser));
        if (compileFunc == null) throw new ArgumentNullException(nameof(compileFunc));

        this.expressionCache = new DictionaryCache<string, TExpression>(this.GetInternalExpression).WithLock();
        this.delegateCache = new DictionaryCache<string, TDelegate>(str => compileFunc(this.expressionCache[str])).WithLock();
    }


    public Type ExpressionType => typeof(TDelegate);

    public virtual string ExpectedFormat => $"\"{this.ExpressionType.ToCSharpShortName()}\"";

    protected abstract TExpression GetInternalExpression(string value);


    protected TResult TryWrapOperation<TResult>(string value, Func<TResult> getResult)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        if (getResult == null) throw new ArgumentNullException(nameof(getResult));

        try
        {
            return getResult();
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Can't parse value: \"{value}\". Expected format: {this.ExpectedFormat}", nameof(value), ex);
        }
    }


    public TExpression GetExpression(string value) => this.TryWrapOperation(value, () => this.expressionCache[value]);

    public TDelegate GetDelegate(string value) => this.TryWrapOperation(value, () => this.delegateCache[value]);

    public virtual void Validate(string source) => this.GetExpression(source);
}
