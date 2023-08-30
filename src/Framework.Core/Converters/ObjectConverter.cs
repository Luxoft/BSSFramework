namespace Framework.Core;

public class ObjectConverter<TSource, TTarget> : ExpressionConverter<TSource, TTarget>, IConverter<TSource, TTarget>
{
    private readonly Lazy<Func<TSource, TTarget>> _lazyConverFunc;

    public ObjectConverter(ILambdaCompileCache compileCache = null)
    {
        this.CompileCache = compileCache ?? Default.CompileCache;

        this._lazyConverFunc = LazyHelper.Create(() => this.GetConvertExpression().Compile(this.CompileCache));
    }


    internal protected override ExpressionConverter<TSubSource, TSubTarget> GetSubConverter<TSubSource, TSubTarget>()
    {
        return new ObjectConverter<TSubSource, TSubTarget>(this.CompileCache);
    }

    protected virtual ILambdaCompileCache CompileCache { get; private set; }


    public TTarget Convert(TSource source)
    {
        return this._lazyConverFunc.Value(source);
    }


    public static readonly ObjectConverter<TSource, TTarget> Default = new ObjectConverter<TSource, TTarget>(new LambdaCompileCache());
}


public static class ObjectConverter
{
    public static object Convert (object source, Type targetType)
    {
        return new Func<object, object>(Convert<object, object>).CreateGenericMethod(source.GetType(), targetType).Invoke(null, new object[] { source });
    }

    private static TTarget Convert<TSource, TTarget>(this TSource source)
    {
        return ConvertHelper<TSource, TTarget>.Converter.Convert(source);
    }

    private static class ConvertHelper<TSource, TTarget>
    {
        public static readonly ObjectConverter<TSource, TTarget> Converter = new ObjectConverter<TSource, TTarget>(new LambdaCompileCache());
    }
}
