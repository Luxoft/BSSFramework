using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven.Serialization;
using Framework.Transfering;

using CommonFramework;

namespace Framework.DomainDriven.DTOGenerator;

public class FileType : IEquatable<FileType>
{
    public readonly string Name;


    public FileType(Expression<Func<FileType>> expr)
            : this(expr.GetStaticMemberName())
    {
    }

    protected FileType(string name)
    {
        this.Name = name ?? throw new ArgumentNullException(nameof(name));
    }


    public string ShortName => this.Name.SkipLast("DTO", true);


    public static readonly MainDTOFileType BaseAbstractDTO = new MainDTOFileType(() => BaseAbstractDTO, () => null, () => BaseAuditPersistentDTO, true);

    public static readonly MainDTOFileType BasePersistentDTO = new MainDTOFileType(() => BasePersistentDTO, () => BaseAbstractDTO, () => BaseAuditPersistentDTO, true);

    public static readonly MainDTOFileType BaseAuditPersistentDTO = new MainDTOFileType(() => BaseAuditPersistentDTO, () => BasePersistentDTO, () => SimpleDTO, true);


    public static readonly MainDTOFileType SimpleDTO = new MainDTOFileType(() => SimpleDTO, () => BaseAuditPersistentDTO, () => FullDTO, false);

    public static readonly MainDTOFileType FullDTO = new MainDTOFileType(() => FullDTO, () => SimpleDTO, () => RichDTO, false);

    public static readonly MainDTOFileType RichDTO = new MainDTOFileType(() => RichDTO, () => FullDTO, () => null, false);


    public static readonly MainDTOFileType VisualDTO = new MainDTOFileType(() => VisualDTO, () => BasePersistentDTO, () => null, false);

    public static readonly DTOFileType ProjectionDTO = new DTOFileType(() => ProjectionDTO, DTORole.Client);


    public static readonly DTOFileType IdentityDTO = new DTOFileType(() => IdentityDTO, DTORole.Client);

    public static readonly DTOFileType StrictDTO = new DTOFileType(() => StrictDTO, DTORole.Client);

    public static readonly DTOFileType UpdateDTO = new DTOFileType(() => UpdateDTO, DTORole.Client);


    public static readonly RoleFileType ClientDTOMappingServiceInterface = new RoleFileType(() => ClientDTOMappingServiceInterface, DTORole.Client);

    public static readonly RoleFileType ClientPrimitiveDTOMappingServiceBase = new RoleFileType(() => ClientPrimitiveDTOMappingServiceBase, DTORole.Client);

    public static readonly RoleFileType ClientPrimitiveDTOMappingService = new RoleFileType(() => ClientPrimitiveDTOMappingService, DTORole.Client);


    public virtual bool Equals(FileType other)
    {
        return !ReferenceEquals(other, null) && this.Name.Equals(other.Name, StringComparison.CurrentCultureIgnoreCase);
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as FileType);
    }

    public override int GetHashCode()
    {
        return this.Name.ToLower().GetHashCode();
    }

    public override string ToString()
    {
        return this.Name;
    }


    public static bool operator ==(FileType fileType, FileType other)
    {
        return ReferenceEquals(fileType, other)
               || (!ReferenceEquals(fileType, null) && fileType.Equals(other));
    }

    public static bool operator !=(FileType fileType, FileType other)
    {
        return !(fileType == other);
    }

    public static implicit operator FileType(DTOType dtoType)
    {
        switch (dtoType)
        {
            case DTOType.IdentityDTO:
                return IdentityDTO;

            case DTOType.VisualDTO:
                return VisualDTO;

            case DTOType.SimpleDTO:
                return SimpleDTO;

            case DTOType.FullDTO:
                return FullDTO;

            case DTOType.RichDTO:
                return RichDTO;

            case DTOType.StrictDTO:
                return StrictDTO;

            case DTOType.UpdateDTO:
                return UpdateDTO;

            case DTOType.ProjectionDTO:
                return ProjectionDTO;

            default:
                throw new ArgumentOutOfRangeException(nameof(dtoType));
        }
    }

    public static implicit operator FileType(MainDTOType dtoType)
    {
        switch (dtoType)
        {
            case MainDTOType.VisualDTO:
                return VisualDTO;

            case MainDTOType.SimpleDTO:
                return SimpleDTO;

            case MainDTOType.FullDTO:
                return FullDTO;

            case MainDTOType.RichDTO:
                return RichDTO;

            default:
                throw new ArgumentOutOfRangeException(nameof(dtoType));
        }
    }

    public static implicit operator FileType(ViewDTOType dtoType)
    {
        switch (dtoType)
        {
            case ViewDTOType.VisualDTO:
                return VisualDTO;

            case ViewDTOType.SimpleDTO:
                return SimpleDTO;

            case ViewDTOType.FullDTO:
                return FullDTO;

            case ViewDTOType.RichDTO:
                return RichDTO;

            case ViewDTOType.ProjectionDTO:
                return ProjectionDTO;

            default:
                throw new ArgumentOutOfRangeException(nameof(dtoType));
        }
    }
}
