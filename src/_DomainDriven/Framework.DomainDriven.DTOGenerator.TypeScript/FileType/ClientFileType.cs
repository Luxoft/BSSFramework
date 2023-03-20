using System;
using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven.Serialization;

namespace Framework.DomainDriven.DTOGenerator.TypeScript;

/// <summary>
/// Client file type
/// </summary>
public static class ClientFileType
{
    public static readonly RoleFileType Enum = new RoleFileType(() => Enum, DTORole.Client);

    public static readonly DTOFileType Struct = new DTOFileType(() => Struct, DTORole.Client);

    public static readonly DTOFileType Class = new DTOFileType(() => Class, DTORole.Client);

    public static readonly MainDTOFileType ProjectionDTO = new MainDTOFileType(() => ProjectionDTO, () => null, () => null, false);

    public static readonly MainDTOInterfaceFileType BaseAbstractInterfaceDTO = new MainDTOInterfaceFileType(() => BaseAbstractInterfaceDTO, FileType.BaseAbstractDTO, null);

    public static readonly MainDTOInterfaceFileType BasePersistentInterfaceDTO = new MainDTOInterfaceFileType(() => BasePersistentInterfaceDTO, FileType.BasePersistentDTO, ClientFileType.BaseAbstractInterfaceDTO);

    public static readonly MainDTOInterfaceFileType BaseAuditPersistentInterfaceDTO = new MainDTOInterfaceFileType(() => BaseAuditPersistentInterfaceDTO, FileType.BaseAuditPersistentDTO, ClientFileType.BasePersistentInterfaceDTO);

    public static readonly MainDTOInterfaceFileType SimpleInterfaceDTO = new MainDTOInterfaceFileType(() => SimpleInterfaceDTO, FileType.SimpleDTO, ClientFileType.BaseAuditPersistentInterfaceDTO);

    public static readonly MainDTOInterfaceFileType FullInterfaceDTO = new MainDTOInterfaceFileType(() => FullInterfaceDTO, FileType.FullDTO, ClientFileType.SimpleInterfaceDTO);

    public static readonly MainDTOInterfaceFileType RichInterfaceDTO = new MainDTOInterfaceFileType(() => RichInterfaceDTO, FileType.RichDTO, ClientFileType.FullInterfaceDTO);
}

/// <summary>
/// MainDTO Interface file type
/// </summary>
public class MainDTOInterfaceFileType : DTOFileType, IMainDTOFileType
{
    public MainDTOInterfaceFileType(Expression<Func<MainDTOInterfaceFileType>> expr, MainDTOFileType mainType, MainDTOInterfaceFileType baseType)
            : this(expr.GetStaticMemberName(), mainType, baseType)
    {
    }

    protected MainDTOInterfaceFileType(string name, MainDTOFileType mainType, MainDTOInterfaceFileType baseType)
            : base(name, DTORole.Client)
    {
        if (mainType == null)
        {
            throw new ArgumentNullException(nameof(mainType));
        }

        this.MainType = mainType;
        this.BaseType = baseType;
    }

    public MainDTOFileType MainType { get; }

    public MainDTOInterfaceFileType BaseType { get; }
}
