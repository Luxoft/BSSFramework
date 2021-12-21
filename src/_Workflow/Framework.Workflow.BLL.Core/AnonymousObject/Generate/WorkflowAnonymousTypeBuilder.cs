using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using Framework.Core;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public class WorkflowAnonymousTypeBuilder : AnonymousTypeByPropertyBuilder<TypeMap<ParameterizedTypeMapMember>, ParameterizedTypeMapMember>
    {
        public WorkflowAnonymousTypeBuilder(IAnonymousTypeBuilderStorage storage)
            : base(storage)
        {

        }


        protected override PropertyBuilder ImplementMember(TypeBuilder typeBuilder, ParameterizedTypeMapMember member)
        {
            var propertyBuilder = base.ImplementMember(typeBuilder, member);

            if (!member.AllowNull && member.Type.IsClass)
            {
                propertyBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(RequiredAttribute).GetConstructor(Type.EmptyTypes), new object[0]));
            }

            if (member.Name == WorkflowParameter.DomainObjectName || member.Name == WorkflowParameter.OwnerWorkflowName)
            {
                propertyBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(DetailRoleAttribute).GetConstructor(new [] { typeof(bool) }), new object[] { false }));
            }

            return propertyBuilder;
        }


        protected override TypeBuilder DefineType(TypeMap<ParameterizedTypeMapMember> typeMap)
        {
            var typeBuilder = base.DefineType(typeMap);

            typeMap.Members.SingleOrDefault(member => member.Name == WorkflowParameter.DomainObjectName).Maybe(domainObjectMember =>
                typeBuilder.AddInterfaceImplementation(typeof(IDomainObjectContainer<>).MakeGenericType(domainObjectMember.Type)));

            typeMap.Members.SingleOrDefault(member => member.Name == WorkflowParameter.OwnerWorkflowName).Maybe(ownerWorkflowMember =>
                typeBuilder.AddInterfaceImplementation(typeof(IOwnerWorkflowContainer<>).MakeGenericType(ownerWorkflowMember.Type)));

            return typeBuilder;
        }

        protected override void PostBuildType(TypeMap<ParameterizedTypeMapMember> typeMap, TypeBuilder typeBuilder, PropertyBuilder[] properties)
        {
            var domainObjectProperty = properties.SingleOrDefault(property => property.Name == WorkflowParameter.DomainObjectName);

            if (domainObjectProperty != null)
            {
                var interfaceProp = typeof(IDomainObjectContainer<>).MakeGenericType(domainObjectProperty.PropertyType).GetProperties().Single();

                typeBuilder.DefineMethodOverride(domainObjectProperty.GetGetMethod(), interfaceProp.GetGetMethod());
            }

            var ownerWorkflowProperty = properties.SingleOrDefault(property => property.Name == WorkflowParameter.OwnerWorkflowName);

            if (ownerWorkflowProperty != null)
            {
                var interfaceProp = typeof(IOwnerWorkflowContainer<>).MakeGenericType(ownerWorkflowProperty.PropertyType).GetProperties().Single();

                typeBuilder.DefineMethodOverride(ownerWorkflowProperty.GetGetMethod(), interfaceProp.GetGetMethod());
            }

            if (domainObjectProperty == null && ownerWorkflowProperty != null)
            {
                var ownerWorkflowDomainObjectType = ownerWorkflowProperty.PropertyType.GetInterfaceImplementationArguments(typeof(IDomainObjectContainer<>), args => args.SingleOrDefault());

                if (ownerWorkflowDomainObjectType != null)
                {
                    var domainObjectContainerType = typeof(IDomainObjectContainer<>).MakeGenericType(ownerWorkflowDomainObjectType);
                    var domainObjectContainerTypeGetMethod = domainObjectContainerType.GetProperties().Single().GetGetMethod();

                    typeBuilder.AddInterfaceImplementation(domainObjectContainerType);

                    var propertyBuilder = typeBuilder.DefineProperty(WorkflowParameter.DomainObjectName, PropertyAttributes.None, ownerWorkflowDomainObjectType, Type.EmptyTypes);

                    {
                        var getMethod = typeBuilder.DefineMethod("get_" + WorkflowParameter.DomainObjectName, MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final, ownerWorkflowDomainObjectType, Type.EmptyTypes);

                        var ilGenerator = getMethod.GetILGenerator();
                        ilGenerator.Emit(OpCodes.Ldarg_0);
                        ilGenerator.Emit(OpCodes.Callvirt, ownerWorkflowProperty.GetGetMethod());
                        ilGenerator.Emit(OpCodes.Castclass, domainObjectContainerType);
                        ilGenerator.Emit(OpCodes.Callvirt, domainObjectContainerTypeGetMethod);
                        ilGenerator.Emit(OpCodes.Ret);

                        propertyBuilder.SetGetMethod(getMethod);

                        typeBuilder.DefineMethodOverride(getMethod, domainObjectContainerTypeGetMethod);
                    }
                }
            }
        }
    }
}