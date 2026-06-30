using System.Linq.Expressions;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.Domain;

public class TargetSystemRootFilterModel : DomainObjectMultiFilterModel<TargetSystem>
{
    public bool? IsMain
    {
        get; set;
    }

    public bool? IsRevision
    {
        get; set;
    }

    protected override IEnumerable<Expression<Func<TargetSystem, bool>>> ToFilterExpressionItems()
    {
        var isMain = this.IsMain;

        if (isMain is not null)
        {
            yield return targetSystem => targetSystem.IsMain == isMain;
        }

        var isRevision = this.IsRevision;

        if (isRevision is not null)
        {
            yield return targetSystem => targetSystem.IsRevision == isRevision;
        }
    }
}

