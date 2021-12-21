using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.DomainDriven.DTOGenerator
{
    public class GenerateTypeMap
    {
        public GenerateTypeMap(Type domainType, DTOFileType fileType, IEnumerable<GeneratePropertyMap> properties)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));
            if (properties == null) throw new ArgumentNullException(nameof(properties));

            this.DomainType = domainType;
            this.FileType = fileType;
            this.Properties = properties.ToArray();
        }

        public Type DomainType { get; }

        public DTOFileType FileType { get; }

        public IReadOnlyCollection<GeneratePropertyMap> Properties { get; }

        /// <summary>
        /// Проверка на использование подтипа в каком-либо свойстве
        /// </summary>
        /// <param name="elementType">Тип элемента свойства</param>
        /// <param name="elementFileType">Тип DTO свойства</param>
        /// <param name="isDetail">Свойство является деталью</param>
        /// <returns></returns>
        public bool UsedDetailRole(Type elementType, RoleFileType elementFileType, bool? isDetail)
        {
            return this.GetNotSelfProperties().Any(prop =>
                prop.ElementType == elementType
             && (elementFileType == null || prop.ElementFileType == null || prop.ElementFileType == elementFileType)
             && (isDetail == null || prop.IsDetail == isDetail.Value));
        }

        private IEnumerable<GeneratePropertyMap> GetNotSelfProperties()
        {
            return this.Properties.Where(prop => prop.ElementType != this.DomainType || prop.ElementFileType != this.FileType);
        }

        public override string ToString()
        {
            return $"DomainType: {this.DomainType.Name} | FileType: {this.FileType}";
        }
    }
}
