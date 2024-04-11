namespace Framework.Authorization.Domain;

public class EntityTypeRootFilterModel : DomainObjectRootFilterModel<SecurityContextType>
{
    public bool IsFilter { get; set; }

    public override System.Linq.Expressions.Expression<System.Func<SecurityContextType, bool>> ToFilterExpression()
    {
        var filtrable = this.IsFilter;

        return entityType => entityType.IsFilter == filtrable;
    }
}
