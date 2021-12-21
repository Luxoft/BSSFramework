using System;

namespace Framework.DomainDriven.BLL
{
    public class BLLContextTypeData
    {
        public BLLContextTypeData(Type contextType, Type persistentDomainObjectBaseType, Type identType)
        {
            this.ContextType = contextType;
            this.PersistentDomainObjectBaseType = persistentDomainObjectBaseType;
            this.IdentType = identType;
        }


        public Type ContextType { get; private set; }

        public Type PersistentDomainObjectBaseType { get; private set; }

        public Type IdentType { get; private set; }
    }
}