using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Persistent;

namespace Framework.DomainDriven.SerializeMetadata
{
    internal class DomainTypePropertySelectLoader : DomainTypePropertyExpandLoader
    {
        private readonly bool _isFullSelect;


        public DomainTypePropertySelectLoader(SystemMetadata systemMetadata, bool isFullSelect)
            : base(systemMetadata)
        {
            this._isFullSelect = isFullSelect;
        }


        protected override IPropertyMetadata GetDomainTypeProperty(IDomainTypeMetadata domainType, string propertyName)
        {
            if (this._isFullSelect)
            {
                return base.GetDomainTypeProperty(domainType, propertyName);
            }
            else
            {
                return domainType.Properties.GetByName(propertyName, false)
                       ?? this.GetDomainTypeSimpleProperties(domainType).GetByName(propertyName);
            }
        }

        protected override IEnumerable<PropertySubsetMetadata> GetDomainTypeProperties(IDomainTypeMetadata domainType, Tuple<string, string>[][] paths)
        {
            var basePropeties = base.GetDomainTypeProperties(domainType, paths);

            var simplePropeties = this.GetDomainTypeSimpleProperties(domainType);

            if (this._isFullSelect)
            {
                return basePropeties.Concat(simplePropeties);
            }
            else
            {
                return paths.Any() ? basePropeties : simplePropeties;
            }
        }

        private IEnumerable<PropertySubsetMetadata> GetDomainTypeSimpleProperties(IDomainTypeMetadata domainType)
        {
            return from property in this.SystemMetadata.GetDomainType(domainType.Type).Properties

                   where !property.IsCollection

                   let propertyType = this.SystemMetadata.Types.GetById(property.TypeHeader)

                   where !propertyType.Role.HasSubset()

                   select new PropertySubsetMetadata(property.Name, propertyType, property.IsCollection, property.AllowNull, property.IsVirtual, property.IsSecurity, property.IsVisualIdentity, null);
        }
    }
}