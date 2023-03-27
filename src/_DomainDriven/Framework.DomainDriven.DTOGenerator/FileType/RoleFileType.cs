using System.ComponentModel;
using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven.Serialization;

namespace Framework.DomainDriven.DTOGenerator;

public class RoleFileType : FileType
{
    public readonly DTORole Role;

    public RoleFileType(Expression<Func<RoleFileType>> expr, DTORole role)
            : this(expr.GetStaticMemberName(), role)
    {
    }

    public RoleFileType(string name, DTORole role)
            : base(name)
    {
        if (!Enum.IsDefined(typeof(DTORole), role)) throw new InvalidEnumArgumentException(nameof(role), (int)role, typeof(DTORole));

        this.Role = role;
    }
}
