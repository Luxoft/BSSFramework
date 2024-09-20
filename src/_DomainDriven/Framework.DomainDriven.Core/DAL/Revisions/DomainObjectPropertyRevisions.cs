namespace Framework.DomainDriven.DAL.Revisions;

public class DomainObjectPropertyRevisions<TIdent, TProperty>(TIdent identity, string propertyName)
    : DomainObjectRevisionBase<TIdent, PropertyRevision<TIdent, TProperty>>(identity),
      IDomainObjectPropertyRevisionBase<TIdent, PropertyRevision<TIdent, TProperty>>
{
    public string PropertyName => propertyName;
}
