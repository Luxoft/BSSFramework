using System;

namespace Framework.DomainDriven.UnitTest.Mock
{
    public class DALTestConfiguration<TIdent>
    {
        public static DALTestConfiguration Create<TDomainObject>(IMockDAL<TDomainObject, TIdent> mockDAL)
        {
            return new DALTestConfiguration(typeof(TDomainObject), mockDAL);
        }
        
    }
    public struct DALTestConfiguration
    {

        public DALTestConfiguration(Type domainObjectType, IMockDAL dal) : this()
        {
            this.DAL = dal;
            this.DomainObjectType = domainObjectType;
        }

        public Type DomainObjectType { get; private set; }
        public IMockDAL DAL { get; private set; }

    }
}