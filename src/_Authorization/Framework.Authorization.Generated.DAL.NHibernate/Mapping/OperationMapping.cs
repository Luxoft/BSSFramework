using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Authorization.Generated.DAL.NHibernate.Mapping
{
    public class OperationMapping : AuthBaseMap<Operation>
    {
        public OperationMapping()
        {
            this.Map(x => x.Name).Unique().Not.Nullable();
            this.Map(x => x.Description);
            this.References(x => x.ApproveOperation).Column($"{nameof(Operation.ApproveOperation)}Id");

            this.HasMany(x => x.Links).AsSet().Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
