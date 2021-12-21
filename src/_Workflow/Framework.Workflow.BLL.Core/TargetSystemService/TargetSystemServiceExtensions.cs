using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.Persistent;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public static class TargetSystemServiceExtensions
    {
        private static IEnumerable<ParameterizedTypeMapMember> GetTypeMapMembers(this ITypeResolver<DomainType> typeResolver, IParametersContainer<Parameter> parameters)
        {
            if (typeResolver == null) throw new ArgumentNullException(nameof(typeResolver));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            return from parameter in parameters.Parameters

                   let baseDomainType = typeResolver.Resolve(parameter.Type, true)

                   let isNullable = parameter.AllowNull && baseDomainType.IsValueType

                   let parameterType = isNullable ? typeof(Nullable<>).MakeGenericType(baseDomainType) : baseDomainType

                   select new ParameterizedTypeMapMember(parameter.Name, parameterType, parameter.AllowNull);
        }

        private static IEnumerable<ParameterizedTypeMapMember> GetTypeMapMembers(this ITargetSystemService targetSystemService, Framework.Workflow.Domain.Definition.Workflow workflow)
        {
            if (targetSystemService == null) throw new ArgumentNullException(nameof(targetSystemService));
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            if (!workflow.IsRoot)
            {
                yield return new ParameterizedTypeMapMember(WorkflowParameter.OwnerWorkflowName, targetSystemService.WorkflowTypeBuilder.GetAnonymousType(workflow.Owner), false);
            }

            foreach (var member in targetSystemService.TypeResolver.GetTypeMapMembers(workflow))
            {
                yield return member;
            }
        }

        public static TypeMap<ParameterizedTypeMapMember> GetTypeMap<TDomainObject>(this ITargetSystemService targetSystemService, TDomainObject domainObject)
            where TDomainObject : AuditPersistentDomainObjectBase, IVisualIdentityObject, IParametersContainer<Parameter>
        {
            if (targetSystemService == null) throw new ArgumentNullException(nameof(targetSystemService));
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            var name = $"{typeof (TDomainObject).Name} [Name = {domainObject.Name}] [Id = {domainObject.Id}] [Version = {domainObject.Version}]";

            var members = targetSystemService.TypeResolver.GetTypeMapMembers(domainObject);

            return new TypeMap<ParameterizedTypeMapMember>(name, members);
        }


        public static string GetTypeName(this Framework.Workflow.Domain.Definition.Workflow workflow)
        {
            return $"Workflow [Name = {workflow.Name}] [Id = {workflow.Id}] [Version = {workflow.Version}]{workflow.Owner.Maybe(ownerWf => string.Format(" | Owner" + ownerWf.GetTypeName()))}";
        }

        public static TypeMap<ParameterizedTypeMapMember> GetTypeMap(this ITargetSystemService targetSystemService, Framework.Workflow.Domain.Definition.Workflow workflow)
        {
            if (targetSystemService == null) throw new ArgumentNullException(nameof(targetSystemService));
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            var name = workflow.GetTypeName();

            var members = targetSystemService.GetTypeMapMembers(workflow);

            return new TypeMap<ParameterizedTypeMapMember>(name, members);
        }

    }
}