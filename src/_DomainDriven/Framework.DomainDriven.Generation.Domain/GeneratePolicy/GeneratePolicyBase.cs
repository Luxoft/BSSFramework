namespace Framework.DomainDriven.Generation.Domain;

public abstract class GeneratePolicy<TIdent> : IGeneratePolicy<TIdent>
{
    public abstract bool Used(Type type, TIdent fileType);

    public static IGeneratePolicy<TIdent> AllowAll = new FuncGeneratePolicy<TIdent>((_, __) => true);

    public static IGeneratePolicy<TIdent> DisableAll = new FuncGeneratePolicy<TIdent>((_, __) => false);
}
