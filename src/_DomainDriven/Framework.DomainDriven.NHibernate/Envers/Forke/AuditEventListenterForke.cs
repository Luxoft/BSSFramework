using NHibernate.Engine;
using NHibernate.Envers.Entities;
using NHibernate.Envers.Event;
using NHibernate.Envers.Exceptions;
using NHibernate.Envers.Synchronization;
using NHibernate.Envers.Synchronization.Work;
using NHibernate.Envers.Tools;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;

namespace NHibernate.Envers.Patch.Forke;

//TODO: fork only for fork AddModUnit. #IADFRAME-1676
// https://nhibernate.jira.com/projects/NHE/issues/NHE-166
public class AuditEventListenerForke : AuditEventListener
{
    public AuditEventListenerForke()
    {
    }

    public override void OnPostInsert(PostInsertEvent evt)
    {
        var entityName = evt.Persister.EntityName;
        if (!this.VerCfg.EntCfg.IsVersioned(entityName)) return;
        checkIfTransactionInProgress(evt.Session);

        var auditProcess = this.VerCfg.AuditProcessManager.Get(evt.Session);

        var workUnit = new AddWorkUnitForke(evt.Session, evt.Persister.EntityName, this.VerCfg,
                                            evt.Id, evt.Persister, evt.State);
        auditProcess.AddWorkUnit(workUnit);
        if (workUnit.ContainsWork())
        {
            this.generateBidirectionalCollectionChangeWorkUnits(auditProcess, evt.Persister, entityName, evt.State,
                                                                null, evt.Session);
        }
    }

    private void generateBidirectionalCollectionChangeWorkUnits(AuditProcess auditProcess,
                                                                IEntityPersister entityPersister,
                                                                string entityName,
                                                                IReadOnlyList<object> newState,
                                                                IReadOnlyList<object> oldState,
                                                                ISessionImplementor session)
    {
        // Checking if this is enabled in configuration ...
        if (!this.VerCfg.GlobalCfg.GenerateRevisionsForCollections)
            return;

        // Checks every property of the entity, if it is an "owned" to-one relation to another entity.
        // If the value of that property changed, and the relation is bi-directional, a new revision
        // for the related entity is generated.
        var propertyNames = entityPersister.PropertyNames;

        for (var i = 0; i < propertyNames.GetLength(0); i++)
        {
            var propertyName = propertyNames[i];
            var relDesc = this.VerCfg.EntCfg.GetRelationDescription(entityName, propertyName);
            if (relDesc != null &&
                relDesc.Bidirectional &&
                relDesc.RelationType == RelationType.ToOne &&
                relDesc.Insertable)
            {
                // Checking for changes
                var oldValue = oldState?[i];
                var newValue = newState?[i];

                if (!Toolz.EntitiesEqual(session, oldValue, newValue))
                {
                    // We have to generate changes both in the old collection (size decreses) and new collection
                    // (size increases).
                    if (newValue != null)
                    {
                        this.addCollectionChangeWorkUnit(auditProcess, session, entityName, relDesc, newValue);
                    }
                    if (oldValue != null)
                    {
                        this.addCollectionChangeWorkUnit(auditProcess, session, entityName, relDesc, oldValue);
                    }
                }
            }
        }
    }

    private void addCollectionChangeWorkUnit(AuditProcess auditProcess, ISessionImplementor session, string fromEntityName, RelationDescription relDesc, object value)
    {
        // relDesc.getToEntityName() doesn't always return the entity name of the value - in case
        // of subclasses, this will be root class, no the actual class. So it can't be used here.
        string toEntityName;
        object id;

        if (value is INHibernateProxy newValueAsProxy)
        {
            toEntityName = session.BestGuessEntityName(value);
            id = newValueAsProxy.HibernateLazyInitializer.Identifier;
            // We've got to initialize the object from the proxy to later read its state.
            value = Toolz.GetTargetFromProxy(session, newValueAsProxy);
        }
        else
        {
            toEntityName = session.GuessEntityName(value);

            var idMapper = this.VerCfg.EntCfg[toEntityName].IdMapper;
            id = idMapper.MapToIdFromEntity(value);
        }

        var toPropertyNames = this.VerCfg.EntCfg.ToPropertyNames(fromEntityName, relDesc.FromPropertyName, toEntityName);
        var toPropertyName = (string)toPropertyNames.First();
        auditProcess.AddWorkUnit(new CollectionChangeWorkUnit(session, toEntityName, toPropertyName, this.VerCfg, id, value));
    }

    private static void checkIfTransactionInProgress(ISessionImplementor session)
    {
        if (!session.TransactionInProgress && session.TransactionContext == null)
        {
            // Historical data would not be flushed to audit tables if outside of active transaction
            // (AuditProcess#doBeforeTransactionCompletion(SessionImplementor) not executed).
            throw new AuditException("Unable to create revision because of non-active transaction");
        }
    }
}
