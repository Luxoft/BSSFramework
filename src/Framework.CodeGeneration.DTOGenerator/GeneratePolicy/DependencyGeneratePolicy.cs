using Framework.CodeGeneration.DTOGenerator.FileType;
using Framework.CodeGeneration.DTOGenerator.Map;
using Framework.CodeGeneration.GeneratePolicy;

namespace Framework.CodeGeneration.DTOGenerator.GeneratePolicy;

public class DependencyGeneratePolicy : CachedGeneratePolicy<RoleFileType>
{
    public DependencyGeneratePolicy(IGeneratePolicy<RoleFileType> baseGeneratePolicy, IEnumerable<GenerateTypeMap> maps)
            : base (baseGeneratePolicy)
    {
        if (baseGeneratePolicy == null) throw new ArgumentNullException(nameof(baseGeneratePolicy));
        if (maps == null) throw new ArgumentNullException(nameof(maps));

        this.Maps = maps.ToArray();

        this.DomainTypes = this.Maps.Select(map => map.DomainType).Distinct().ToArray();
    }


    protected IReadOnlyCollection<Type> DomainTypes { get; }

    protected IReadOnlyCollection<GenerateTypeMap> Maps { get; }


    protected bool IsUsedProperty(DTOFileType ownerFileType, Type elementType, RoleFileType elementFileType, bool? detailFlag = null)
    {
        if (elementFileType == null) throw new ArgumentNullException(nameof(elementFileType));
        if (elementType == null) throw new ArgumentNullException(nameof(elementType));

        var elementMap = this.Maps.SingleOrDefault(map => map.DomainType == elementType && map.FileType == elementFileType);

        var usedInTypes = this.Maps.Where(map => (ownerFileType == null || map.FileType == ownerFileType)) // Выкидываем лишние MAP-ы
                              .Where(map => map != elementMap) // Выкидываем Map текущего элемента
                              .Where(map => map.UsedDetailRole(elementType, elementFileType, detailFlag) && this.Used(map.DomainType, map.FileType));

        return usedInTypes.Any();
    }

    protected override bool InternalUsed(Type domainType, RoleFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        if (fileType == FileType.FileType.BaseAbstractDTO)
        {
            return true;// this._maps.Any(map => (map.FileType == FileType.BasePersistentDTO || map.FileType == FileType.BaseVisualDTO) && this.Used(map.DomainType, map.FileType));
        }
        else if (fileType == FileType.FileType.BasePersistentDTO)
        {
            return true;
        }
        else if (fileType == FileType.FileType.BaseAuditPersistentDTO)
        {
            return true;
        }
        else if (base.InternalUsed(domainType, fileType))
        {
            return true;
        }
        else if (fileType == FileType.FileType.ProjectionDTO)
        {
            return this.IsUsedProperty(FileType.FileType.ProjectionDTO, domainType, fileType);
        }
        else if (fileType == FileType.FileType.StrictDTO)
        {
            return this.IsUsedProperty(FileType.FileType.StrictDTO, domainType, fileType, true)
                   || this.Used(domainType, FileType.FileType.UpdateDTO);
        }
        else if (fileType == FileType.FileType.UpdateDTO)
        {
            return this.IsUsedProperty(FileType.FileType.UpdateDTO, domainType, fileType, true);
        }
        else if (fileType == FileType.FileType.IdentityDTO)
        {
            return this.IsUsedProperty(FileType.FileType.StrictDTO, domainType, fileType, false)
                   || this.IsUsedProperty(FileType.FileType.UpdateDTO, domainType, fileType, false)
                   || this.IsUsedProperty(FileType.FileType.UpdateDTO, domainType, FileType.FileType.UpdateDTO, true);
        }
        else if (fileType == FileType.FileType.RichDTO)
        {
            return this.IsUsedProperty(FileType.FileType.RichDTO, domainType, fileType, true);
        }
        else if (fileType == FileType.FileType.FullDTO)
        {
            return this.Used(domainType, FileType.FileType.RichDTO);
        }
        else if (fileType == FileType.FileType.SimpleDTO)
        {
            return this.Used(domainType, FileType.FileType.FullDTO)

                   || this.IsUsedProperty(FileType.FileType.RichDTO, domainType, fileType, false)

                   || this.IsUsedProperty(FileType.FileType.FullDTO, domainType, fileType, false);
        }
        else
        {
            return false;
        }
    }
}
