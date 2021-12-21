using System;
using System.Collections.Generic;

namespace Framework.DomainDriven.Metadata
{
    public class PrimitiveTypeFieldMetadata : FieldMetadata
    {
        private readonly bool _isIdentity;
        private readonly bool _isCollection;

        public PrimitiveTypeFieldMetadata(
            string name,
            Type type,
            IEnumerable<Attribute> attributes,
            DomainTypeMetadata domainTypeMetadata,
            bool isIdentity,
            bool isCollection = false)
            : base(name, type, attributes, domainTypeMetadata)
        {
            this._isIdentity = isIdentity;
            this._isCollection = isCollection;
        }

        public bool IsCollection
        {
            get { return this._isCollection; }
        }

        public bool IsIdentity
        {
            get { return this._isIdentity; }
        }
    }
}
