using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.DAL.Revisions;
using Framework.DomainDriven.ServiceModel.Subscriptions;
using Framework.Persistent;
using Framework.Security;
using Framework.Transfering;
using Framework.Validation;

namespace Framework.DomainDriven.ServiceModel.Service;

public class AuditService<TIdent, TBllContext, TBllFactoryContainer, TRootSecurityService, TPersistentObjectBase,
                          TDomainPropertyRevisionsDto, TPropertyRevisionDto>(TBllContext bllContext)
    where TDomainPropertyRevisionsDto : DomainObjectPropertiesRevisionDTO<TIdent, TPropertyRevisionDto>, new()
    where TPropertyRevisionDto : PropertyRevisionDTOBase
    where TPersistentObjectBase : class, IIdentityObject<TIdent>
    where TBllFactoryContainer : IBLLFactoryContainer<IDefaultBLLFactory<TPersistentObjectBase, TIdent>>
    where TBllContext : IBLLFactoryContainerContext<TBllFactoryContainer>,
    ITypeResolverContainer<string>,
    ISecurityServiceContainer<TRootSecurityService>
    where TRootSecurityService : IRootSecurityService<TPersistentObjectBase>
{
    private static readonly Lazy<Type> GenericTPropertyRevisionDtoType = new Lazy<Type>(
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
            var dtoType = bllContext.TypeResolver.Resolve(typeof(TProperty).Name + MainDTOType.SimpleDTO.ToString(), true);

            var method =
                    ((Func<PropertyRevision<TIdent, TProperty>, TPropertyRevisionDto>)
                        (this.ToPropertyRevisionDto<object, TProperty>))
                    .CreateGenericMethod(dtoType, typeof(TProperty));

            result.RevisionInfos = propertyChanged.RevisionInfos.Select(z => (TPropertyRevisionDto)method.InvokeWithExceptionProcessed(this, z)).ToList();
        }
        else
        {
            result.RevisionInfos = propertyChanged.RevisionInfos.Select(z => this.CreateGenericProperptyRevisionDto(z, z.Value)).ToList();
        }

        return result;
    }
    private bool HasAccess<TDomain>(TDomain domainObject, PropertyInfo propertyInfo)
            where TDomain : class, TPersistentObjectBase
    {
        var viewSecurityRule = propertyInfo.GetViewSecurityRule();

        return bllContext.SecurityService.GetSecurityProvider<TDomain>(viewSecurityRule).HasAccess(domainObject);
    }

    private TPropertyRevisionDto ToPropertyRevisionDto<TDtoProperty, TProperty>(
            PropertyRevision<TIdent, TProperty> propertyRevision)
    {
        if (null == propertyRevision.Value)
        {
            return this.CreateGenericProperptyRevisionDto(propertyRevision, default(TDtoProperty));
        }

        var dtoPropertyValue =
                (TDtoProperty)
                Activator.CreateInstance(typeof(TDtoProperty), bllContext, propertyRevision.Value);

        return this.CreateGenericProperptyRevisionDto(propertyRevision, dtoPropertyValue);
    }

    private TPropertyRevisionDto CreateGenericProperptyRevisionDto<TValue>(RevisionInfoBase propertyRevisionInfo, TValue value)
    {
        var genericType = GenericTPropertyRevisionDtoType.Value.MakeGenericType(typeof(TValue));

        var result = (TPropertyRevisionDto)Activator.CreateInstance(genericType, propertyRevisionInfo);

        genericType.GetProperty("Value", BindingFlags.Public | BindingFlags.Instance).SetValue(result, value);

        return result;
    }

}
