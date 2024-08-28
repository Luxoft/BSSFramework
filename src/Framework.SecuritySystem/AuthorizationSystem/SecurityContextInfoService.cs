namespace Framework.SecuritySystem;

public class SecurityContextInfoService : ISecurityContextInfoService
{
    private readonly IReadOnlyDictionary<Type, ISecurityContextInfo> byTypeSecurityContextInfoDict;

    private readonly IReadOnlyDictionary<Guid, ISecurityContextInfo> byIdentSecurityContextInfoDict;

    public SecurityContextInfoService(IEnumerable<ISecurityContextInfo> securityContextInfoList)
    {
        this.byTypeSecurityContextInfoDict = securityContextInfoList.ToDictionary(v => v.Type);
        this.byIdentSecurityContextInfoDict = this.byTypeSecurityContextInfoDict.Values.ToDictionary(v => v.Id);

        this.SecurityContextTypes = this.byTypeSecurityContextInfoDict.Keys.ToList();
    }

    public IReadOnlyList<Type> SecurityContextTypes { get; }

    public virtual ISecurityContextInfo GetSecurityContextInfo(Type type) => this.byTypeSecurityContextInfoDict[type];

    ISecurityContextInfo ISecurityContextInfoService.GetSecurityContextInfo(Type type) => this.GetSecurityContextInfo(type);

    public ISecurityContextInfo GetSecurityContextInfo(Guid ident) => this.byIdentSecurityContextInfoDict[ident];
}
