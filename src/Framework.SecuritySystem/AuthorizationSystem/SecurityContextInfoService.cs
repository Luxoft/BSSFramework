namespace Framework.SecuritySystem;

public class SecurityContextInfoService<TIdent> : ISecurityContextInfoService<TIdent>
    where TIdent : notnull
{
    private readonly IReadOnlyDictionary<Type, ISecurityContextInfo<TIdent>> byTypeSecurityContextInfoDict;

    private readonly IReadOnlyDictionary<TIdent, ISecurityContextInfo<TIdent>> byIdentSecurityContextInfoDict;

    public SecurityContextInfoService(IEnumerable<ISecurityContextInfo<TIdent>> securityContextInfoList)
    {
        this.byTypeSecurityContextInfoDict = securityContextInfoList.ToDictionary(v => v.Type);
        this.byIdentSecurityContextInfoDict = this.byTypeSecurityContextInfoDict.Values.ToDictionary(v => v.Id);

        this.SecurityContextTypes = this.byTypeSecurityContextInfoDict.Keys.ToList();
    }

    public IReadOnlyList<Type> SecurityContextTypes { get; }

    public virtual ISecurityContextInfo<TIdent> GetSecurityContextInfo(Type type) => this.byTypeSecurityContextInfoDict[type];

    ISecurityContextInfo ISecurityContextInfoService.GetSecurityContextInfo(Type type) => this.GetSecurityContextInfo(type);

    public ISecurityContextInfo<TIdent> GetSecurityContextInfo(TIdent ident) => this.byIdentSecurityContextInfoDict[ident];
}
