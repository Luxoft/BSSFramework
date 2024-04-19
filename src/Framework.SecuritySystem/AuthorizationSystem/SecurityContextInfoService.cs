namespace Framework.SecuritySystem;

public class SecurityContextInfoService : ISecurityContextInfoService
{
    private readonly IReadOnlyDictionary<Type, ISecurityContextInfo> byTypeSecurityContextInfoDict;

    private readonly IReadOnlyDictionary<string, ISecurityContextInfo> byNameSecurityContextInfoDict;

    public SecurityContextInfoService(IEnumerable<ISecurityContextInfo> securityContextInfoList)
    {
        this.byTypeSecurityContextInfoDict = securityContextInfoList.ToDictionary(v => v.Type);
        this.byNameSecurityContextInfoDict = this.byTypeSecurityContextInfoDict.Values.ToDictionary(v => v.Name);

        this.SecurityContextTypes = this.byTypeSecurityContextInfoDict.Keys.ToList();
    }

    public IReadOnlyList<Type> SecurityContextTypes { get; }

    public virtual ISecurityContextInfo GetSecurityContextInfo(Type type) => this.byTypeSecurityContextInfoDict[type];

    public ISecurityContextInfo GetSecurityContextInfo(string name) => this.byNameSecurityContextInfoDict[name];
}
