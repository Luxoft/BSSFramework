using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Serialization;

namespace Framework.CodeGeneration.DTOGenerator.FileTypes;

public record BaseFileType(string Name)
{
    public string ShortName => this.Name.SkipLast("DTO", true);


    public static MainDTOFileType BaseAbstractDTO { get; } = new (nameof (BaseAbstractDTO), null, true);

    public static MainDTOFileType BasePersistentDTO { get; } = new (nameof (BasePersistentDTO), BaseAbstractDTO, true);

    public static MainDTOFileType BaseAuditPersistentDTO { get; } = new (nameof (BaseAuditPersistentDTO), BasePersistentDTO, true);


    public static MainDTOFileType SimpleDTO { get; } = new (nameof (SimpleDTO), BaseAuditPersistentDTO, false);

    public static MainDTOFileType FullDTO { get; } = new (nameof (FullDTO), SimpleDTO, false);

    public static MainDTOFileType RichDTO { get; } = new (nameof (RichDTO), FullDTO, false);


    public static MainDTOFileType VisualDTO { get; } = new(nameof(VisualDTO), BasePersistentDTO, false);

    public static DTOFileType ProjectionDTO { get; } = new (nameof (ProjectionDTO), DTORole.Client);


    public static DTOFileType IdentityDTO { get; } = new(nameof(IdentityDTO), DTORole.Client);

    public static DTOFileType StrictDTO { get; } = new(nameof(StrictDTO), DTORole.Client);

    public static DTOFileType UpdateDTO { get; } = new(nameof(UpdateDTO), DTORole.Client);


    public static RoleFileType ClientDTOMappingServiceInterface { get; } = new(nameof(ClientDTOMappingServiceInterface), DTORole.Client);

    public static RoleFileType ClientPrimitiveDTOMappingServiceBase { get; } = new(nameof(ClientPrimitiveDTOMappingServiceBase), DTORole.Client);

    public static RoleFileType ClientPrimitiveDTOMappingService { get; } = new(nameof(ClientPrimitiveDTOMappingService), DTORole.Client);

    public sealed override string ToString() => this.InternalToString();

    protected virtual string InternalToString() => this.Name;

    public static implicit operator BaseFileType(DTOType dtoType)
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

    public static implicit operator BaseFileType(MainDTOType dtoType)
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

    public static implicit operator BaseFileType(ViewDTOType dtoType)
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
