using System;
using System.Linq.Expressions;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.DomainDriven.DTOGenerator.TypeScript
{
    /// <summary>
    /// Observable file type
    /// </summary>
    public static class ObservableFileType
    {
        public static readonly ObservableMainDTOFileType BaseObservableAbstractDTO = new ObservableMainDTOFileType(() => BaseObservableAbstractDTO, () => null, () => BaseObservablePersistentDTO, true, FileType.BaseAbstractDTO);

        public static readonly ObservableMainDTOFileType BaseObservablePersistentDTO = new ObservableMainDTOFileType(() => BaseObservablePersistentDTO, () => BaseObservableAbstractDTO, () => ObservableSimpleDTO, false, FileType.BasePersistentDTO);

        public static readonly ObservableMainDTOFileType BaseObservableAuditPersistentDTO = new ObservableMainDTOFileType(() => BaseObservableAuditPersistentDTO, () => BaseObservablePersistentDTO, () => ObservableSimpleDTO, false, FileType.BaseAuditPersistentDTO);

        public static readonly ObservableMainDTOFileType ObservableSimpleDTO = new ObservableMainDTOFileType(() => ObservableSimpleDTO, () => BaseObservableAuditPersistentDTO, () => ObservableFullDTO, false, FileType.SimpleDTO);

        public static readonly ObservableMainDTOFileType ObservableFullDTO = new ObservableMainDTOFileType(() => ObservableFullDTO, () => ObservableSimpleDTO, () => ObservableRichDTO, false, FileType.FullDTO);

        public static readonly ObservableMainDTOFileType ObservableRichDTO = new ObservableMainDTOFileType(() => ObservableRichDTO, () => ObservableFullDTO, () => null, false, FileType.RichDTO);

        public static readonly ObservableMainDTOFileType ObservableVisualDTO = new ObservableMainDTOFileType(() => ObservableVisualDTO, () => null, () => null, false, FileType.VisualDTO);

        public static readonly ObservableMainDTOFileType ObservableProjectionDTO = new ObservableMainDTOFileType(() => ObservableProjectionDTO, () => null, () => null, false, ClientFileType.ProjectionDTO);

        public static readonly MainDTOInterfaceFileType BaseObservablePersistentInterfaceDTO =
                new MainDTOInterfaceFileType(() => BaseObservablePersistentInterfaceDTO, BaseObservablePersistentDTO, null);

        public static readonly MainDTOInterfaceFileType BaseObservableAuditPersistentInterfaceDTO =
                new MainDTOInterfaceFileType(() => BaseObservableAuditPersistentInterfaceDTO, BaseObservableAuditPersistentDTO, BaseObservablePersistentInterfaceDTO);

        public static readonly MainDTOInterfaceFileType ObservableSimpleInterfaceDTO =
                new MainDTOInterfaceFileType(() => ObservableSimpleInterfaceDTO, ObservableSimpleDTO, BaseObservableAuditPersistentInterfaceDTO);

        public static readonly MainDTOInterfaceFileType ObservableFullInterfaceDTO =
                new MainDTOInterfaceFileType(() => ObservableFullInterfaceDTO, ObservableFullDTO, ObservableSimpleInterfaceDTO);

        public static readonly MainDTOInterfaceFileType ObservableRichInterfaceDTO =
                new MainDTOInterfaceFileType(() => ObservableRichInterfaceDTO, ObservableRichDTO, ObservableFullInterfaceDTO);

        public static readonly MainDTOFileType ObservableIdentityDTO = new MainDTOFileType(() => ObservableIdentityDTO, () => null, () => null, false);
    }

    /// <summary>
    /// Observable mainDTO file type
    /// </summary>
    public class ObservableMainDTOFileType : MainDTOFileType, IMainDTOFileType
    {
        [NotNull]
        public MainDTOFileType UnwrapType { get; }

        public ObservableMainDTOFileType(Expression<Func<ObservableMainDTOFileType>> expr, Func<ObservableMainDTOFileType> getBaseType, Func<ObservableMainDTOFileType> getNestedType, bool isAbstract, MainDTOFileType unwrapType)
            : this(expr.GetStaticMemberName(), getBaseType, getNestedType, isAbstract, unwrapType)
        {
        }

        protected ObservableMainDTOFileType(string name, Func<ObservableMainDTOFileType> getBaseType, Func<ObservableMainDTOFileType> getNestedType, bool isAbstract, [NotNull] MainDTOFileType unwrapType)
            : base(name, getBaseType, getNestedType, isAbstract)
        {
            if (unwrapType == null)
            {
                throw new ArgumentNullException(nameof(unwrapType));
            }

            this.UnwrapType = unwrapType;
        }
    }
}
