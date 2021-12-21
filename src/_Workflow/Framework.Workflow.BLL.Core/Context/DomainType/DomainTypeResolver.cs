using System;
using System.Linq;
using System.Collections.Generic;

using Framework.Core;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public class DomainTypeResolver : ITypeResolver<DomainType>
    {
        private readonly ITypeResolver<string> _domainTypeResolver;


        public DomainTypeResolver(ITypeResolver<string> domainTypeResolver)
        {
            if (domainTypeResolver == null) throw new ArgumentNullException(nameof(domainTypeResolver));

            this._domainTypeResolver = domainTypeResolver;
        }


        public Type Resolve(DomainType identity)
        {
            if (identity == null) throw new ArgumentNullException(nameof(identity));

            switch (identity.Role)
            {
                case DomainTypeRole.Primitive:

                    return TypeResolverHelper.Base.Resolve(identity.FullTypeName, true);

                case DomainTypeRole.Domain:

                    return this._domainTypeResolver.Resolve(identity.FullTypeName, true);

                case DomainTypeRole.Other:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentOutOfRangeException(nameof(identity), "invalid role");
            }
        }

        public IEnumerable<Type> GetTypes()
        {
            return TypeResolverHelper.Base.GetTypes().Concat(this._domainTypeResolver.GetTypes());
        }
    }
}