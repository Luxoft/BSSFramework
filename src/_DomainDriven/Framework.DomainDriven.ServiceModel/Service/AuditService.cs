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

public class AuditService<TIdent, TBLLContext, TBLLFactoryContainer, TRootSecurityService, TPersistentObjectBase, TDomainPropertyRevisionsDTO, TPropertyRevisionDTO>
        where TDomainPropertyRevisionsDTO : DomainObjectPropertiesRevisionDTO<TIdent, TPropertyRevisionDTO>, new()
        where TPropertyRevisionDTO : PropertyRevisionDTOBase
        where TPersistentObjectBase : class, IIdentityObject<TIdent>
        where TBLLFactoryContainer : IBLLFactoryContainer<IDefaultBLLFactory<TPersistentObjectBase, TIdent>>
        where TBLLContext : IBLLFactoryContainerContext<TBLLFactoryContainer>,
        ITypeResolverContainer<string>,
        ISecurityServiceContainer<TRootSecurityService>
        where TRootSecurityService : IRootSecurityService<TPersistentObjectBase>
{
    private readonly static Lazy<Type> _genericTPropertyRevisionDTOType = new Lazy<Type>(
     () => typeof(TPropertyRevisionDTO)
           .Assembly
           .GetTypes()
           .Where(z => z.IsGenericType)
           .Single(z => typeof(TPropertyRevisionDTO).IsAssignableFrom(z)), true);


    private readonly TBLLContext _bllContext;



    public AuditService(TBLLContext bllContext)
    {
        this._bllContext = bllContext;
    }


    public TDomainPropertyRevisionsDTO GetPropertyChanges<TDomain>(TIdent id, string propertyName, Period? period = null)
            where TDomain : class, TPersistentObjectBase
    {
        var propertyInfo = typeof(TDomain).GetProperties().FirstOrDefault(z => string.Equals(z.Name, propertyName, StringComparison.InvariantCultureIgnoreCase));

        if (null == propertyInfo)
        {
            throw new ValidationException("{0} {1}", typeof(TDomain).Name, propertyName);
        }

        var methodDefinition = (((Func<TIdent, string, PropertyInfo, Period?, TDomainPropertyRevisionsDTO>)this.GetPropertyChanged<TDomain, object>))
                .CreateGenericMethod(typeof(TDomain), propertyInfo.PropertyType);

        var result = methodDefinition.InvokeWithExceptionProcessed(this, id, propertyName, propertyInfo, period);

        return (TDomainPropertyRevisionsDTO)result;

    }

    private TDomainPropertyRevisionsDTO GetPropertyChanged<TDomain, TProperty>(TIdent id, string propertyName, PropertyInfo propertyInfo, Period? period = null)
            where TDomain : class, TPersistentObjectBase
    {

        var propertyChanged = this._bllContext.Logics.Default.Create<TDomain>().GetPropertyChanges<TProperty>(id, propertyName, period);

        var domainObject = this._bllContext.Logics.Default.Create<TDomain>().GetById(id); //????

        var result = new TDomainPropertyRevisionsDTO
        {
            Identity = propertyChanged.Identity,
            PropertyName = propertyChanged.PropertyName,
        };

        if (propertyInfo.IsSecurity())
        {
            if (!this.HassAccess(domainObject, propertyInfo))
            {
                return result;
            }
        }

        if (typeof(TPersistentObjectBase).IsAssignableFrom(typeof(TProperty)))
        {
            var dtoType = this._bllContext.TypeResolver.Resolve(typeof(TProperty).Name + MainDTOType.SimpleDTO.ToString(), true);

            var method =
                    ((Func<PropertyRevision<TIdent, TProperty>, TPropertyRevisionDTO>)
                        (this.ToPropertyRevisionDTO<object, TProperty>))
                    .CreateGenericMethod(dtoType, typeof(TProperty));

            result.RevisionInfos = propertyChanged.RevisionInfos.Select(z => (TPropertyRevisionDTO)method.InvokeWithExceptionProcessed(this, z)).ToList();
        }
        else
        {
            result.RevisionInfos = propertyChanged.RevisionInfos.Select(z => this.CreateGenericProperptyRevisionDTO(z, z.Value)).ToList();
        }

        return result;
    }
    private bool HassAccess<TDomain>(TDomain domainObject, PropertyInfo propertyInfo)
            where TDomain : class, TPersistentObjectBase
    {
        var viewOperation = propertyInfo.GetViewDomainObjectCode();

        var castedViewOperation =
                (TSecurityOperationCode)Convert.ChangeType(viewOperation, typeof(TSecurityOperationCode));

        return this._bllContext.SecurityService.GetSecurityProvider<TDomain>(castedViewOperation).HasAccess(domainObject);
    }

    private TPropertyRevisionDTO ToPropertyRevisionDTO<TDTOProperty, TProperty>(
            PropertyRevision<TIdent, TProperty> propertyRevision)
    {
        if (null == propertyRevision.Value)
        {
            return this.CreateGenericProperptyRevisionDTO(propertyRevision, default(TDTOProperty));
        }

        var dtoPropertyValue =
                (TDTOProperty)
                Activator.CreateInstance(typeof(TDTOProperty), this._bllContext, propertyRevision.Value);

        return this.CreateGenericProperptyRevisionDTO(propertyRevision, dtoPropertyValue);
    }

    private TPropertyRevisionDTO CreateGenericProperptyRevisionDTO<TValue>(RevisionInfoBase propertyRevisionInfo, TValue value)
    {
        var genericType = _genericTPropertyRevisionDTOType.Value.MakeGenericType(typeof(TValue));

        var result = (TPropertyRevisionDTO)Activator.CreateInstance(genericType, propertyRevisionInfo);

        genericType.GetField("Value", BindingFlags.Public | BindingFlags.Instance).SetValue(result, value);

        return result;
    }

}
