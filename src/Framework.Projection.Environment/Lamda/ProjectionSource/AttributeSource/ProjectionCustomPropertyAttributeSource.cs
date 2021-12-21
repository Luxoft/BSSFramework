﻿using System;
using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda
{
    /// <summary>
    /// Источник атрибутов для кастомного свойства проекции
    /// </summary>
    public class ProjectionCustomPropertyAttributeSource : AttributeSourceBase<IProjectionCustomProperty>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="environment">Окружение</param>
        /// <param name="customProjectionProperty">Кастомное свойство проекции</param>
        public ProjectionCustomPropertyAttributeSource([NotNull] ProjectionLambdaEnvironment environment, [NotNull] IProjectionCustomProperty customProjectionProperty)
            : base(environment, customProjectionProperty)
        {
        }


        protected override IEnumerable<Attribute> GetDefaultAttributes()
        {
            return new Attribute[]
            {
                this.TryCreateIgnoreFetchAttribute(),
                this.CreateProjectionPropertyAttribute()
            }.Where(attr => attr != null).Concat(this.GetFetchPathAttributes());
        }

        protected virtual IgnoreFetchAttribute TryCreateIgnoreFetchAttribute()
        {
            if (this.ProjectionValue.Fetchs.Any())
            {
                return null;
            }
            else
            {
                return new IgnoreFetchAttribute();
            }
        }

        protected virtual ProjectionPropertyAttribute CreateProjectionPropertyAttribute()
        {
            return new ProjectionPropertyAttribute(ProjectionPropertyRole.Custom);
        }

        private IEnumerable<FetchPathAttribute> GetFetchPathAttributes()
        {
            foreach (var path in this.ProjectionValue.Fetchs)
            {
                yield return new FetchPathAttribute(path);
            }
        }
    }
}
