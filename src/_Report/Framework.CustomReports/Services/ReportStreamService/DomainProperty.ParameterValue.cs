using System;

using Framework.DomainDriven.SerializeMetadata;

namespace Framework.CustomReports.Services
{
    public struct DomainProperty
    {
        public DomainProperty(Type domainType, PropertyMetadata propertyMetadata)
        {
            this.DomainType = domainType;
            this.PropertyMetadata = propertyMetadata;
        }

        public PropertyMetadata PropertyMetadata { get; private set; }

        public Type DomainType { get; private set; }
    }
}