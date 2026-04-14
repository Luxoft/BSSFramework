using System.Linq.Expressions;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.Domain;

public class DomainTypeRootFilterModel : DomainObjectFilterModel<DomainType>
{
    public TargetSystem TargetSystem { get; set; }


    public override Expression<Func<DomainType, bool>> ToFilterExpression()
    {
        var targetSystem = this.TargetSystem;

        return domainType => targetSystem == null || targetSystem == domainType.TargetSystem;
    }
}
