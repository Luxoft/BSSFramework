namespace Framework.Security;

[AttributeUsage(AttributeTargets.Field)]
public abstract class ApproveRuleAttribute : Attribute
{
    public readonly Enum Rule;


    protected ApproveRuleAttribute(Enum rule)
    {
        if (rule == null) throw new ArgumentNullException(nameof(rule));

        this.Rule = rule;
    }
}
