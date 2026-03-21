namespace Framework.DomainDriven.Generation.Domain;

public class FuncGeneratePolicy<TIdent> : GeneratePolicy<TIdent>
{
    private readonly Func<Type, TIdent, bool> _func;

    public FuncGeneratePolicy(Func<Type, TIdent, bool> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        this._func = func;
    }

    public override bool Used(Type type, TIdent fileType)
    {
        return this._func(type, fileType);
    }
}
