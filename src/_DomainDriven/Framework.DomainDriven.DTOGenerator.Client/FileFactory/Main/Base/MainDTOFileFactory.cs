using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.Security;

namespace Framework.DomainDriven.DTOGenerator.Client
{

    public abstract class MainDTOFileFactoryBase<TConfiguration> : DTOFileFactory<TConfiguration, MainDTOFileType>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        protected MainDTOFileFactoryBase(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {

        }



        public MainDTOFileType BaseFileType => this.FileType.GetBaseType(false);


        public override CodeTypeReference BaseReference =>

            this.BaseFileType.Maybe(baseType => this.Configuration.GetCodeTypeReference(this.DomainType, baseType));

        public override CodeTypeReference CurrentInterfaceReference =>

            this.Configuration.GetBaseInterfaceType(this.FileType).Maybe(interfaceFileType => this.Configuration.GetCodeTypeReference(this.DomainType, interfaceFileType));


        protected sealed override bool? InternalBaseTypeContainsPropertyChange => this.BaseFileType != null;



        protected sealed override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                IsClass = true,
                IsPartial = true,
                TypeAttributes = this.FileType.IsAbstract ? (TypeAttributes.Public | TypeAttributes.Abstract) : TypeAttributes.Public
            };
        }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            if (this.IsPersistent())
            {
                yield return this.CurrentReference.GeneratePersistentCloneMethodWithCopyIdParameter();
            }
        }

        protected override IEnumerable<CodeConstructor> GetConstructors()
        {
            foreach (var baseCtor in base.GetConstructors())
            {
                yield return baseCtor;
            }

            yield return this.GenerateDefaultConstructor();

            if (this.IsPersistent())
            {
                yield return this.GeneratePersistentCloneConstructor();

                yield return this.GeneratePersistentCloneConstructorWithCopyIdParameter();
            }
            else
            {
                yield return this.GenerateUnpersistentCloneConstructor();
            }
        }
    }

    public abstract class MainDTOFileFactory<TConfiguration> : MainDTOFileFactoryBase<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        protected MainDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
            this.CodeTypeReferenceService = new MainCodeTypeReferenceService<TConfiguration>(this.Configuration);
        }



        public override IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }



        protected override CodeMemberProperty CreatePropertyMember(System.Reflection.PropertyInfo sourceProperty, CodeMemberField fieldMember, Func<CodeExpression, CodeExpression> overrideMethodInfo, CodeMethodReferenceExpression raisePropertyChangingMethodReference, CodeMethodReferenceExpression raisePropertyChangedMethodReference)
        {
            var prop = base.CreatePropertyMember(sourceProperty, fieldMember, overrideMethodInfo, raisePropertyChangingMethodReference, raisePropertyChangedMethodReference);

            return prop;
        }

        protected override IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            foreach (var baseType in base.GetBaseTypes())
            {
                yield return baseType;
            }

            yield return new CodeTypeReference(typeof(ICloneable<>)) { TypeArguments = { this.CurrentReference } };
        }


        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }


            yield return CodeDomHelper.GenerateExplicitImplementationClone();

            yield return this.CurrentReference.GenerateCloneMethod(this.FileType.BaseType.Maybe(v => !v.IsAbstract));



            //foreach (var fileType in this.FileType.GetBaseTypes().Concat(new[] { this.FileType }))
            //{
            //    yield return this.GenerateConvertMethod(fileType);
            //}

            yield return this.GenerateConvertMethod(this.FileType);

            if (this.Configuration.GeneratePolicy.Used(this.DomainType, DTOGenerator.FileType.StrictDTO))
            {
                yield return this.GenerateConvertMethod(DTOGenerator.FileType.StrictDTO);
            }
        }

        protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
        {
            foreach (var customAttribute in base.GetCustomAttributes())
            {
                yield return customAttribute;
            }

            foreach (var knownTypesAttribute in this.Configuration.GetKnownTypesAttributes(this.FileType, this.DomainType))
            {
                yield return knownTypesAttribute;
            }
        }

        protected override System.Collections.Generic.IEnumerable<CodeAttributeDeclaration> GetPropertyCustomAttributes(System.Reflection.PropertyInfo sourceProperty)
        {
            if (sourceProperty == null) throw new ArgumentNullException(nameof(sourceProperty));

            foreach (var customAttribute in base.GetPropertyCustomAttributes(sourceProperty))
            {
                yield return customAttribute;
            }

            foreach (var attr in sourceProperty.GetDomainObjectAccessAttributes())
            {
                yield return attr.ToCodeAttributeDeclaration();
            }


            foreach (var attrDecl in sourceProperty.GetRestrictionCodeAttributeDeclarations())
            {
                yield return attrDecl;
            }
        }
    }
}