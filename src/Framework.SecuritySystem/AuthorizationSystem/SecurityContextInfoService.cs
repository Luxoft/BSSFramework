namespace Framework.SecuritySystem;

public class SecurityContextInfoService : ISecurityContextInfoService
{
    private readonly IReadOnlyDictionary<Type, SecurityContextInfo> byTypeSecurityContextInfoDict;

    private readonly IReadOnlyDictionary<string, SecurityContextInfo> byNameSecurityContextInfoDict;

    public SecurityContextInfoService(IEnumerable<SecurityContextInfo> securityContextInfoList)
    {
        this.byTypeSecurityContextInfoDict = securityContextInfoList.ToDictionary(v => v.Type);
        this.byNameSecurityContextInfoDict = this.byTypeSecurityContextInfoDict.Values.ToDictionary(v => v.Name);
    }

    public virtual SecurityContextInfo GetSecurityContextInfo(Type type) => this.byTypeSecurityContextInfoDict[type];

    public SecurityContextInfo GetSecurityContextInfo(string name) => this.byNameSecurityContextInfoDict[name];
}
