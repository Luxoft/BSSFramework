using System;
using System.Collections.Generic;

using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.TypeScript
{
    /// <summary>
    /// Политика вычисляющая используемые типы по косвенным зависимостям
    /// </summary>
    public class TypeScriptDependencyGeneratePolicy : DependencyGeneratePolicy
    {
        public TypeScriptDependencyGeneratePolicy(IGeneratePolicy<RoleFileType> baseGeneratePolicy, IEnumerable<GenerateTypeMap> maps)
            : base(baseGeneratePolicy, maps)
        {
        }

        protected override bool InternalUsed(Type domainType, RoleFileType fileType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));
            if (fileType == null) throw new ArgumentNullException(nameof(fileType));

            if (fileType == ObservableFileType.BaseObservableAbstractDTO)
            {
                return true;
            }
            else if (fileType == ObservableFileType.BaseObservablePersistentDTO)
            {
                return true;
            }
            else if (fileType == ObservableFileType.BaseObservableAuditPersistentDTO)
            {
                return true;
            }
            else if (base.InternalUsed(domainType, fileType))
            {
                return true;
            }
            else if (fileType is MainDTOInterfaceFileType interfaceFileType)
            {
                return this.Used(domainType, interfaceFileType.MainType);
            }
            else if (fileType == ClientFileType.Enum || fileType == ClientFileType.Class || fileType == ClientFileType.Struct)
            {
                return this.IsUsedProperty(null, domainType, fileType);
            }
            else if (fileType == ObservableFileType.ObservableProjectionDTO)
            {
                return this.Used(domainType, FileType.ProjectionDTO);
            }
            else if (fileType == ObservableFileType.ObservableRichDTO)
            {
                return this.Used(domainType, FileType.RichDTO);
            }
            else if (fileType == ObservableFileType.ObservableFullDTO)
            {
                return this.Used(domainType, FileType.FullDTO);
            }
            else if (fileType == ObservableFileType.ObservableSimpleDTO)
            {
                return this.Used(domainType, FileType.SimpleDTO);
            }
            else if (fileType == ObservableFileType.BaseObservableAuditPersistentInterfaceDTO)
            {
                return this.Used(domainType, ObservableFileType.BaseObservableAuditPersistentDTO);
            }
            else if (fileType == ObservableFileType.BaseObservablePersistentInterfaceDTO)
            {
                return this.Used(domainType, ObservableFileType.BaseObservablePersistentDTO);
            }
            else if (fileType == ObservableFileType.ObservableSimpleInterfaceDTO)
            {
                return this.Used(domainType, ObservableFileType.ObservableSimpleDTO);
            }
            else if (fileType == ObservableFileType.ObservableFullInterfaceDTO)
            {
                return this.Used(domainType, ObservableFileType.ObservableFullDTO);
            }
            else if (fileType == ObservableFileType.ObservableRichInterfaceDTO)
            {
                return this.Used(domainType, ObservableFileType.ObservableRichDTO);
            }
            else if (fileType == ObservableFileType.ObservableIdentityDTO)
            {
                return this.Used(domainType, FileType.IdentityDTO);
            }
            else if (fileType == ObservableFileType.ObservableVisualDTO)
            {
                return this.Used(domainType, FileType.VisualDTO);
            }
            else
            {
                return false;
            }
        }
    }
}
