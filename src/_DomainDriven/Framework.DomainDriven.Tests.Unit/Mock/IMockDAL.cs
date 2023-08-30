namespace Framework.DomainDriven.UnitTest.Mock;

public interface IMockDAL
{
    void Register(object value);
    void Flush();
}

public interface IMockDAL<TDomainObject, TIdent> : IMockDAL, IDAL<TDomainObject, TIdent>
{
}
