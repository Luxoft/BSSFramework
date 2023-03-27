using NHibernate.Engine;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Synchronization.Work;
using NHibernate.Persister.Entity;

namespace NHibernate.Envers.Patch.Forke;

//TODO: Remove after fix Envers bugs (// https://nhibernate.jira.com/projects/NHE/issues/NHE-166)
public class AddWorkUnitForke : AbstractAuditWorkUnit
{
    private readonly IEntityPersister entityPersister;

    private readonly IDictionary<string, object> data;

    public AddWorkUnitForke(ISessionImplementor sessionImplementor, string entityName, AuditConfiguration verCfg,
                            object id, IEntityPersister entityPersister, object[] state)
            : base(sessionImplementor, entityName, verCfg, id, RevisionType.Added)
    {
        this.entityPersister = entityPersister;
        this.State = state;
        this.data = new Dictionary<string, object>();
        verCfg.EntCfg[this.EntityName].PropertyMapper.Map(sessionImplementor, this.data,
                                                          entityPersister.PropertyNames, state, null);
    }

    private AddWorkUnitForke(ISessionImplementor sessionImplementor, string entityName, AuditConfiguration verCfg,
                             object id, IDictionary<string, object> data) : base(sessionImplementor, entityName, verCfg, id, RevisionType.Added)
    {
        this.data = data;
    }

    internal object[] State { get; }

    public override bool ContainsWork()
    {
        return true;
    }

    public override IDictionary<string, object> GenerateData(object revisionData)
    {
        this.FillDataWithId(this.data, revisionData);
        return this.data;
    }

    public override IAuditWorkUnit Merge(AddWorkUnit second)
    {
        return second;
    }

    public override IAuditWorkUnit Merge(ModWorkUnit second)
    {
        return new AddWorkUnitForke(this.SessionImplementor, this.EntityName, this.VerCfg, this.EntityId, this.mergeModifiedFlags(this.data, second.Data));
    }

    public override IAuditWorkUnit Merge(DelWorkUnit second)
    {
        return null;
    }

    public override IAuditWorkUnit Merge(CollectionChangeWorkUnit second)
    {
        second.MergeCollectionModifiedData(this.data);
        return this;
    }

    public override IAuditWorkUnit Merge(FakeBidirectionalRelationWorkUnit second)
    {
        return FakeBidirectionalRelationWorkUnit.Merge(second, this, second.NestedWorkUnit);
    }

    public override IAuditWorkUnit Dispatch(IWorkUnitMergeVisitor first)
    {
        return first.Merge(new AddWorkUnit(this.SessionImplementor, this.EntityName, this.VerCfg, this.EntityId, this.entityPersister, this.State));
    }

    private IDictionary<string, object> mergeModifiedFlags(IDictionary<string, object> lhs, IDictionary<string, object> rhs)
    {
        var mapper = this.VerCfg.EntCfg[this.EntityName].PropertyMapper;
        foreach (var propertyData in mapper.Properties.Keys)
        {
            if (propertyData.UsingModifiedFlag)
            {
                object lhsUntypedValue;
                if (lhs.TryGetValue(propertyData.ModifiedFlagPropertyName, out lhsUntypedValue))
                {
                    var lhsValue = (bool)lhsUntypedValue;
                    if (lhsValue)
                    {
                        rhs[propertyData.ModifiedFlagPropertyName] = true;
                    }
                }
            }
        }
        return rhs;
    }
}
