
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

        if (fileType == BaseFileType.BaseAbstractDTO)
        {
            return true;// this._maps.Any(map => (map.FileType == BaseFileType.BasePersistentDTO || map.FileType == BaseFileType.BaseVisualDTO) && this.Used(map.DomainType, map.FileType));
        }
        else if (fileType == BaseFileType.BasePersistentDTO)
        {
            return true;
        }
        else if (fileType == BaseFileType.BaseAuditPersistentDTO)
        {
            return true;
        }
        else if (base.InternalUsed(domainType, fileType))
        {
            return true;
        }
        else if (fileType == BaseFileType.ProjectionDTO)
        {
            return this.IsUsedProperty(BaseFileType.ProjectionDTO, domainType, fileType);
        }
        else if (fileType == BaseFileType.StrictDTO)
        {
            return this.IsUsedProperty(BaseFileType.StrictDTO, domainType, fileType, true)
                   || this.Used(domainType, BaseFileType.UpdateDTO);
        }
        else if (fileType == BaseFileType.UpdateDTO)
        {
            return this.IsUsedProperty(BaseFileType.UpdateDTO, domainType, fileType, true);
        }
        else if (fileType == BaseFileType.IdentityDTO)
        {
            return this.IsUsedProperty(BaseFileType.StrictDTO, domainType, fileType, false)
                   || this.IsUsedProperty(BaseFileType.UpdateDTO, domainType, fileType, false)
                   || this.IsUsedProperty(BaseFileType.UpdateDTO, domainType, BaseFileType.UpdateDTO, true);
        }
        else if (fileType == BaseFileType.RichDTO)
        {
            return this.IsUsedProperty(BaseFileType.RichDTO, domainType, fileType, true);
        }
        else if (fileType == BaseFileType.FullDTO)
        {
            return this.Used(domainType, BaseFileType.RichDTO);
        }
        else if (fileType == BaseFileType.SimpleDTO)
        {
            return this.Used(domainType, BaseFileType.FullDTO)

                   || this.IsUsedProperty(BaseFileType.RichDTO, domainType, fileType, false)

                   || this.IsUsedProperty(BaseFileType.FullDTO, domainType, fileType, false);
        }
        else
        {
            return false;
        }
    }
}
