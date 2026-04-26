using System.Reflection;

using Anch.Core;

using Framework.Application.Domain;
using Framework.BLL;
using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Extensions;
using Framework.BLL.DTOMapping.Domain;
using Framework.BLL.Services;
using Framework.Core;
using Framework.Core.TypeResolving;
using Framework.Database.Domain;
using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.Service;

public class AuditService<TIdent, TBLLContext, TBllFactoryContainer, TRootSecurityService, TPersistentObjectBase,
                          TDomainPropertyRevisionsDto, TPropertyRevisionDto>(TBLLContext bllContext)
    where TDomainPropertyRevisionsDto : DomainObjectPropertiesRevisionDTO<TIdent, TPropertyRevisionDto>, new()
    where TPropertyRevisionDto : PropertyRevisionDTOBase
    where TPersistentObjectBase : class, IIdentityObject<TIdent>
    where TBllFactoryContainer : IBLLFactoryContainer<IDefaultBLLFactory<TPersistentObjectBase, TIdent>>
    where TBLLContext : IBLLFactoryContainerContext<TBllFactoryContainer>, ISecurityServiceContainer<TRootSecurityService>, IServiceProviderContainer
    where TRootSecurityService : IRootSecurityService
{
    private static readonly Lazy<Type> GenericTPropertyRevisionDtoType = new(
     () => typeof(TPropertyRevisionDto)
           .Assembly
           .GetTypes()
           .Where(z => z.IsGenericType)
           .Single(z => typeof(TPropertyRevisionDto).IsAssignableFrom(z)), true);

    public TDomainPropertyRevisionsDto GetPropertyChanges<TDomain>(TIdent id, string propertyName, Period? period = null)
            where TDomain : class, TPersistentObjectBase
    {
        var propertyInfo = typeof(TDomain).GetProperties().FirstOrDefault(z => string.Equals(z.Name, propertyName, StringComparison.InvariantCultureIgnoreCase));

        if (null == propertyInfo)
        {
            throw new ValidationException("{0} {1}", typeof(TDomain).Name, propertyName);
        }

        var methodDefinition = (((Func<TIdent, string, PropertyInfo, Period?, TDomainPropertyRevisionsDto>)this.GetPropertyChanged<TDomain, object>))
                .CreateGenericMethod(typeof(TDomain), propertyInfo.PropertyType);

        var result = methodDefinition.InvokeWithExceptionProcessed(this, id, propertyName, propertyInfo, period);

        return (TDomainPropertyRevisionsDto)result;

    }

    private TDomainPropertyRevisionsDto GetPropertyChanged<TDomain, TProperty>(TIdent id, string propertyName, PropertyInfo propertyInfo, Period? period = null)
            where TDomain : class, TPersistentObjectBase
    {

        var propertyChanged = bllContext.Logics.Default.Create<TDomain>().GetPropertyChanges<TProperty>(id, propertyName, period);

        var domainObject = bllContext.Logics.Default.Create<TDomain>().GetById(id); //????

        var result = new TDomainPropertyRevisionsDto
        {
            Identity = propertyChanged.Identity,
            PropertyName = propertyChanged.PropertyName,
        };

        if (propertyInfo.IsSecurity())
        {
            if (!this.HasAccess(domainObject, propertyInfo))
            {
                return result;
            }
        }

        if (typeof(TPersistentObjectBase).IsAssignableFrom(typeof(TProperty)))
        {
            var typeResolver = bllContext.ServiceProvider.GetRequiredKeyedService<ITypeResolver<TypeNameIdentity>>("DTO");

            var dtoType = typeResolver.Resolve(typeof(TProperty).Name + MainDTOType.SimpleDTO);

            var method =
                    ((Func<PropertyRevision<TIdent, TProperty>, TPropertyRevisionDto>)
                        (this.ToPropertyRevisionDto<object, TProperty>))
                    .CreateGenericMethod(dtoType, typeof(TProperty));

            result.RevisionInfos = propertyChanged.RevisionInfos.Select(z => (TPropertyRevisionDto)method.InvokeWithExceptionProcessed(this, z)).ToList();
        }
        else
        {
            result.RevisionInfos = propertyChanged.RevisionInfos.Select(z => this.CreateGenericPropertyRevisionDto(z, z.Value)).ToList();
        }

        return result;
    }
    private bool HasAccess<TDomain>(TDomain domainObject, PropertyInfo propertyInfo)
            where TDomain : class, TPersistentObjectBase
    {
        var viewSecurityRule = propertyInfo.GetViewSecurityRule();

        return bllContext.SecurityService.GetSecurityProvider<TDomain>(viewSecurityRule).HasAccessAsync(domainObject).GetAwaiter().GetResult();
    }

    private TPropertyRevisionDto ToPropertyRevisionDto<TDtoProperty, TProperty>(
            PropertyRevision<TIdent, TProperty> propertyRevision)
    {
        if (null == propertyRevision.Value)
        {
            return this.CreateGenericPropertyRevisionDto(propertyRevision, default(TDtoProperty));
        }

        var dtoPropertyValue =
                (TDtoProperty)
                Activator.CreateInstance(typeof(TDtoProperty), bllContext, propertyRevision.Value)!;

        return this.CreateGenericPropertyRevisionDto(propertyRevision, dtoPropertyValue);
    }

    private TPropertyRevisionDto CreateGenericPropertyRevisionDto<TValue>(RevisionInfoBase propertyRevisionInfo, TValue value)
    {
        var genericType = GenericTPropertyRevisionDtoType.Value.MakeGenericType(typeof(TValue));

        var result = (TPropertyRevisionDto)Activator.CreateInstance(genericType, propertyRevisionInfo)!;

        genericType.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance)!.SetValue(result, value);

        return result;
    }

}
