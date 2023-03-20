using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator;

public class DependencyGeneratePolicy : CachedGeneratePolicy<RoleFileType>
{
    public DependencyGeneratePolicy(IGeneratePolicy<RoleFileType> baseGeneratePolicy, IEnumerable<GenerateTypeMap> maps)
            : base (baseGeneratePolicy)
    {
        if (baseGeneratePolicy == null) throw new ArgumentNullException(nameof(baseGeneratePolicy));
        if (maps == null) throw new ArgumentNullException(nameof(maps));

        this.Maps = maps.ToArray(true);

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

        if (fileType == FileType.BaseAbstractDTO)
        {
            return true;// this._maps.Any(map => (map.FileType == FileType.BasePersistentDTO || map.FileType == FileType.BaseVisualDTO) && this.Used(map.DomainType, map.FileType));
        }
        else if (fileType == FileType.BasePersistentDTO)
        {
            return true;
        }
        else if (fileType == FileType.BaseAuditPersistentDTO)
        {
            return true;
        }
        else if (base.InternalUsed(domainType, fileType))
        {
            return true;
        }
        else if (fileType == FileType.ProjectionDTO)
        {
            return this.IsUsedProperty(FileType.ProjectionDTO, domainType, fileType);
        }
        else if (fileType == FileType.StrictDTO)
        {
            return this.IsUsedProperty(FileType.StrictDTO, domainType, fileType, true)
                   || this.Used(domainType, FileType.UpdateDTO);
        }
        else if (fileType == FileType.UpdateDTO)
        {
            return this.IsUsedProperty(FileType.UpdateDTO, domainType, fileType, true);
        }
        else if (fileType == FileType.IdentityDTO)
        {
            return this.IsUsedProperty(FileType.StrictDTO, domainType, fileType, false)
                   || this.IsUsedProperty(FileType.UpdateDTO, domainType, fileType, false)
                   || this.IsUsedProperty(FileType.UpdateDTO, domainType, FileType.UpdateDTO, true);
        }
        else if (fileType == FileType.RichDTO)
        {
            return this.IsUsedProperty(FileType.RichDTO, domainType, fileType, true);
        }
        else if (fileType == FileType.FullDTO)
        {
            return this.Used(domainType, FileType.RichDTO);
        }
        else if (fileType == FileType.SimpleDTO)
        {
            return this.Used(domainType, FileType.FullDTO)

                   || this.IsUsedProperty(FileType.RichDTO, domainType, fileType, false)

                   || this.IsUsedProperty(FileType.FullDTO, domainType, fileType, false);
        }
        else
        {
            return false;
        }
    }
}
