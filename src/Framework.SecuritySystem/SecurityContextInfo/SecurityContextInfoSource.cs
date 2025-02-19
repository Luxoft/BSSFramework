namespace Framework.SecuritySystem;

public class SecurityContextInfoSource : ISecurityContextInfoSource
{
    private readonly IReadOnlyDictionary<Type, SecurityContextInfo> byTypeSecurityContextInfoDict;

    private readonly IReadOnlyDictionary<Guid, SecurityContextInfo> byIdentSecurityContextInfoDict;

    public SecurityContextInfoSource(IEnumerable<SecurityContextInfo> securityContextInfoList)
    {
        this.SecurityContextInfoList = securityContextInfoList.ToList();
        this.byTypeSecurityContextInfoDict = this.SecurityContextInfoList.ToDictionary(v => v.Type);
        this.byIdentSecurityContextInfoDict = this.byTypeSecurityContextInfoDict.Values.ToDictionary(v => v.Id);
    }
    public IReadOnlyList<SecurityContextInfo> SecurityContextInfoList { get; }

    public virtual SecurityContextInfo GetSecurityContextInfo(Type securityContextType) => this.byTypeSecurityContextInfoDict[securityContextType];

    public SecurityContextInfo GetSecurityContextInfo(Guid securityContextTypeId) => this.byIdentSecurityContextInfoDict[securityContextTypeId];
}
