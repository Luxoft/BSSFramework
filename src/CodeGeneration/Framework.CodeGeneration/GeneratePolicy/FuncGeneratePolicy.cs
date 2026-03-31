namespace Framework.CodeGeneration.GeneratePolicy;

public class FuncGeneratePolicy<TIdent> : GeneratePolicy<TIdent>
{
    private readonly Func<Type, TIdent, bool> func;

    public FuncGeneratePolicy(Func<Type, TIdent, bool> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        this.func = func;
    }

    public override bool Used(Type type, TIdent fileType) => this.func(type, fileType);
}
