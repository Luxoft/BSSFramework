using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultRichDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        private readonly Lazy<ReadOnlyCollection<PropertyInfo>> _lazyDetailsProperties;
        //private readonly bool _isHierarchical;

        public DefaultRichDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
            this._lazyDetailsProperties = LazyHelper.Create(() =>
            {
                var request = from prop in this.GetProperties(false)

                              where !this.CodeTypeReferenceService.IsOptional(prop)

                              let elementType = prop.PropertyType.GetCollectionElementType()

                              where elementType != null

                              where this.DomainType.IsAssignableToAll(typeof(IMaster<>).MakeGenericType(elementType))
                                 && elementType.IsAssignableToAll(typeof(IDetail<>).MakeGenericType(this.DomainType))

                              select prop;

                return request.ToReadOnlyCollection();
            });

            //this._isHierarchical = this.DomainType.IsImplementedInterface(typeof(IChildrenSource<>), this.DomainType)
            //                    && this.DomainType.IsPropertyImplement(typeof(IChildrenSource<>).MakeGenericType(this.DomainType));
        }


        public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.RichDTO;



        protected override CodeExpression GetFieldInitExpression(PropertyInfo property)
        {
            if (!this.CodeTypeReferenceService.IsOptional(property))
            {
                if (property.PropertyType.IsCollection())
                {
                    return this.CodeTypeReferenceService.GetCodeTypeReference(property, true).ToObjectCreateExpression();
                }
            }

            return base.GetFieldInitExpression(property);
        }

        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var member in base.GetMembers())
            {
                yield return member;
            }



            foreach (var detailProperty in this._lazyDetailsProperties.Value)
            {
                var elementType = detailProperty.PropertyType.GetCollectionElementType();

                var elementTypeRef = this.Configuration.GetCodeTypeReference(elementType, DTOGenerator.FileType.RichDTO);

                yield return new CodeMemberProperty
                {
                    Type = typeof(ICollection<>).ToTypeReference(elementTypeRef),
                    PrivateImplementationType = typeof(IMaster<>).ToTypeReference(elementTypeRef),
                    Name = "Details",
                    GetStatements =
                    {
                        new CodeThisReferenceExpression().ToPropertyReference(detailProperty).ToMethodReturnStatement()
                    }
                };
            }

            //if (this._isHierarchical)
            //{
            //    yield return new CodeMemberProperty
            //    {
            //        Type = typeof(IEnumerable<>).ToTypeReference(this.CurrentReference),
            //        PrivateImplementationType = typeof(IChildrenSource<>).ToTypeReference(this.CurrentReference),
            //        Name = "Children",
            //        GetStatements =
            //        {
            //            new CodeThisReferenceExpression().ToPropertyReference("Children").ToMethodReturnStatement()
            //        }
            //    };
            //}
        }

        protected override IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            foreach (var baseType in base.GetBaseTypes())
            {
                yield return baseType;
            }

            foreach (var detailProperty in this._lazyDetailsProperties.Value)
            {
                  var elementType = detailProperty.PropertyType.GetCollectionElementType();

                var elementTypeRef = this.Configuration.GetCodeTypeReference(elementType, DTOGenerator.FileType.RichDTO);

                yield return typeof(IMaster<>).ToTypeReference(elementTypeRef);
            }

            //if (this._isHierarchical)
            //{
            //    yield return typeof(IChildrenSource<>).ToTypeReference(this.CurrentReference);
            //}
        }

        protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
        {
            foreach (var customAttribute in base.GetCustomAttributes())
            {
                yield return customAttribute;
            }

            foreach (var attrDecl in this.DomainType.GetRestrictionCodeAttributeDeclarations())
            {
                yield return attrDecl;
            }
        }
    }
}
