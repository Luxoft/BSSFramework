namespace Framework.Authorization.Domain
{
    public class EntityTypeRootFilterModel : DomainObjectRootFilterModel<EntityType>
    {
        public bool IsFilter { get; set; }

        public override System.Linq.Expressions.Expression<System.Func<EntityType, bool>> ToFilterExpression()
        {
            var filtrable = this.IsFilter;

            return entityType => entityType.IsFilter == filtrable;
        }
    }
}