using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;

using JetBrains.Annotations;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.PropertyAssigner
{
    /// <summary>
    /// Property assigner extensions
    /// </summary>
    public static class PropertyAssignerExtensions
    {
        public static CodeStatement GetAssignStatementBySource(
            this IPropertyAssigner propertyAssigner,
            PropertyInfo property,
            CodeExpression sourceObjectRef,
            CodeExpression targetObjectRef,
            [NotNull] FileType sourceFileType,
            FileType baseFileType)
        {
            if (propertyAssigner == null)
            {
                throw new ArgumentNullException(nameof(propertyAssigner));
            }

            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (sourceObjectRef == null)
            {
                throw new ArgumentNullException(nameof(sourceObjectRef));
            }

            if (targetObjectRef == null)
            {
                throw new ArgumentNullException(nameof(targetObjectRef));
            }

            if (sourceFileType == null)
            {
                throw new ArgumentNullException(nameof(sourceFileType));
            }

            if (sourceFileType == ObservableFileType.BaseObservablePersistentDTO
                || sourceFileType == ObservableFileType.ObservableFullDTO
                || sourceFileType == ObservableFileType.ObservableSimpleDTO
                || sourceFileType == ObservableFileType.ObservableRichDTO)
            {
                if (baseFileType == null || baseFileType == FileType.StrictDTO)
                {
                    return propertyAssigner.GetAssignStatement(property, sourceObjectRef.ToMethodInvokeExpression(property.Name), targetObjectRef.ToPropertyReference(property));
                }

                return targetObjectRef.ToMethodInvokeExpression(property.Name, sourceObjectRef.ToMethodInvokeExpression(property.Name)).ToExpressionStatement();
            }

            return propertyAssigner.GetAssignStatement(property, sourceObjectRef.ToPropertyReference(property), targetObjectRef.ToPropertyReference(property));
        }
    }
}
