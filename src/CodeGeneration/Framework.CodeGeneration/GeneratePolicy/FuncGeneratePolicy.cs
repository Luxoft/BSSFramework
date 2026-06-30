namespace Framework.CodeGeneration.GeneratePolicy;

public class FuncGeneratePolicy<TIdent>(Func<Type, TIdent, bool> func) : GeneratePolicy<TIdent>
{
    private readonly Func<Type, TIdent, bool> func = func ?? throw new ArgumentNullException(nameof(func));

    public override bool Used(Type type, TIdent fileType) => this.func(type, fileType);
}
