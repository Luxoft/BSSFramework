using System;
using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven.Serialization;

namespace Framework.DomainDriven.DTOGenerator
{
    public class DTOFileType : RoleFileType
    {
        public DTOFileType(Expression<Func<DTOFileType>> expr, DTORole role)
            : this(expr.GetStaticMemberName(), role)
        {
        }

        protected DTOFileType(string name, DTORole role)
            : base(name, role)
        {
        }
    }
}