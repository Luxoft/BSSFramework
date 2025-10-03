using System.Linq.Expressions;

using CommonFramework;

using Framework.Core;
using Framework.DomainDriven.Serialization;

namespace Framework.DomainDriven.DTOGenerator;

public class MainDTOFileType : DTOFileType, IMainDTOFileType
{
    private readonly Lazy<MainDTOFileType> _lazyBaseType;

    private readonly Lazy<MainDTOFileType> _lazyNestedType;

    public readonly bool IsAbstract;


    public MainDTOFileType(Expression<Func<MainDTOFileType>> expr, Func<MainDTOFileType> getBaseType, Func<MainDTOFileType> getNestedType, bool isAbstract)
            : this(expr.GetStaticMemberName(), getBaseType, getNestedType, isAbstract)
    {
    }

    protected MainDTOFileType(string name, Func<MainDTOFileType> getBaseType, Func<MainDTOFileType> getNestedType, bool isAbstract)
            : base(name, DTORole.Client)
    {
        this._lazyBaseType = getBaseType.ToLazy();
        this._lazyNestedType = getNestedType.ToLazy();
        this.IsAbstract = isAbstract;
    }


    public MainDTOFileType BaseType => this._lazyBaseType.Value;

    public MainDTOFileType NestedType => this._lazyNestedType.Value;
}
