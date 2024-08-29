namespace Framework.SecuritySystem;

public class SecurityContextSource : ISecurityContextSource
{
    private readonly IReadOnlyDictionary<Type, SecurityContextInfo> byTypeSecurityContextInfoDict;

    private readonly IReadOnlyDictionary<Guid, SecurityContextInfo> byIdentSecurityContextInfoDict;

    public SecurityContextSource(IEnumerable<SecurityContextInfo> securityContextInfoList)
    {
        this.byTypeSecurityContextInfoDict = securityContextInfoList.ToDictionary(v => v.Type);
        this.byIdentSecurityContextInfoDict = this.byTypeSecurityContextInfoDict.Values.ToDictionary(v => v.Id);

        this.SecurityContextTypes = this.byTypeSecurityContextInfoDict.Keys.ToList();
    }

    public IReadOnlyList<Type> SecurityContextTypes { get; }

    public virtual SecurityContextInfo GetSecurityContextInfo(Type type) => this.byTypeSecurityContextInfoDict[type];

    SecurityContextInfo ISecurityContextSource.GetSecurityContextInfo(Type type) => this.GetSecurityContextInfo(type);

    public SecurityContextInfo GetSecurityContextInfo(Guid ident) => this.byIdentSecurityContextInfoDict[ident];
}
