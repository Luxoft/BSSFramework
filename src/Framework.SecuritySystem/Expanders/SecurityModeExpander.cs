namespace Framework.SecuritySystem.Expanders;

public class SecurityModeExpander
{
    private readonly IReadOnlyDictionary<(Type, SecurityRule.ModeSecurityRule), DomainSecurityRule> dict;

    public SecurityModeExpander(IEnumerable<DomainObjectSecurityModeInfo> infos)
    {
        var request = from info in infos

                      from pair in new[] { (Mode: SecurityRule.View, TargetRule: info.ViewRule), (Mode: SecurityRule.Edit, TargetRule: info.EditRule) }

                      where pair.TargetRule != null

                      select ((info.DomainType, pair.Mode), pair.TargetRule);

        this.dict = request.ToDictionary();
    }

    public DomainSecurityRule? TryExpand<TDomainObject>(SecurityRule.ModeSecurityRule securityRule)
    {
        return this.dict.GetValueOrDefault((typeof(TDomainObject), securityRule));
    }
}
