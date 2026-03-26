using System.Linq.Expressions;

using Framework.BLL.Domain.Serialization;
using Framework.Core;

namespace Framework.CodeGeneration.DTOGenerator.FileType;

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
