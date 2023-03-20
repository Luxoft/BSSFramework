namespace Framework.DomainDriven.DAL.Revisions;

public class DomainObjectPropertyRevisions<TIdent, TProperty> : DomainObjectRevisionBase<TIdent, PropertyRevision<TIdent, TProperty>>, IDomainObjectPropertyRevisionBase<TIdent, PropertyRevision<TIdent, TProperty>>
{
    private readonly string _propertyName;

    public DomainObjectPropertyRevisions(TIdent identity, string propertyName) : base(identity)
    {
        this._propertyName = propertyName;
    }

    public string PropertyName
    {
        get { return this._propertyName; }
    }
}
