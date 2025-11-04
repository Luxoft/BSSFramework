namespace Framework.DomainDriven.DAL.Revisions;

public class DomainObjectPropertyRevisions<TIdent>(TIdent identity, string propertyName)
    : DomainObjectRevisionBase<TIdent, PropertyRevision<TIdent, string>>(identity)
{
    public string PropertyName => propertyName;
}

public class DomainObjectPropertyRevisionsBase<TIdent>(TIdent identity, string propertyName)
    : DomainObjectRevisionBase<TIdent, RevisionInfoBase>(identity), IDomainObjectPropertyRevisionBase<TIdent, RevisionInfoBase>
{
    public string PropertyName { get; private set; } = propertyName;
}
