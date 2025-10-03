using System.Linq.Expressions;

using CommonFramework.DictionaryCache;
using CommonFramework.ExpressionEvaluate;

using Framework.Core;

using Framework.Exceptions;

namespace Framework.ExpressionParsers;

public abstract class ExpressionParser<TDelegate>(INativeExpressionParser parser)
    : ExpressionParser<TDelegate, Expression<TDelegate>>(parser, expr => LambdaCompileCache.GetFunc(expr))
    where TDelegate : class
{
    protected override Expression<TDelegate> GetInternalExpression(string value)
    {
        return this.Parser.Parse<TDelegate>(value);
    }


    private static readonly ILambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.None);
}

public abstract class ExpressionParser<TDelegate, TExpression> : NativeExpressionParserContainer,

                                                                 IExpressionParser<string, TDelegate, TExpression>

        where TDelegate : class
        where TExpression : LambdaExpression
{
    private readonly IDictionaryCache<string, TExpression> _expressionCache;

    private readonly IDictionaryCache<string, TDelegate> _delegateCache;


    protected ExpressionParser(INativeExpressionParser parser, Func<TExpression, TDelegate> compileFunc)
            : base (parser)
    {
        if (parser == null) throw new ArgumentNullException(nameof(parser));
        if (compileFunc == null) throw new ArgumentNullException(nameof(compileFunc));

        this._expressionCache = new DictionaryCache<string, TExpression>(this.GetInternalExpression).WithLock();
        this._delegateCache = new DictionaryCache<string, TDelegate>(str => compileFunc(this._expressionCache[str])).WithLock();
    }


    public Type ExpressionType
    {
        get { return typeof(TDelegate); }
    }

    public virtual string ExpectedFormat
    {
        get { return $"\"{this.ExpressionType.ToCSharpShortName()}\""; }
    }


    protected abstract TExpression GetInternalExpression(string value);


    protected TResult TryWrapOperation<TResult>(string value, bool wrapError, Func<TResult> getResult)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        if (getResult == null) throw new ArgumentNullException(nameof(getResult));

        try
        {
            return getResult();
        }
        catch (Exception ex)
        {
            if (wrapError)
            {
                throw new BusinessLogicException(ex, $"Can't parse value: \"{value}\". Expected format: {this.ExpectedFormat}");
            }
            else
            {
                throw;
            }
        }
    }


    public TExpression GetExpression(string value, bool wrapError = true)
    {
        return this.TryWrapOperation(value, wrapError, () => this._expressionCache[value]);
    }

    public TDelegate GetDelegate(string value, bool wrapError = true)
    {
        return this.TryWrapOperation(value, wrapError, () => this._delegateCache[value]);
    }

    public virtual void Validate(string source)
    {
        this.GetExpression(source);
    }
}
