using Framework.BLL.Domain.DirectMode;
using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Serialization.Extensions;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeGeneration.BLLCoreGenerator.Extensions;
using Framework.CodeGeneration.GeneratePolicy;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.Core;
using Framework.Projection;

namespace Framework.CodeGeneration.ServiceModelGenerator.GeneratePolicy;

/// <summary>
/// Стандартная политика для управления генерацией фасадных методов
/// </summary>
public class DefaultServiceGeneratePolicy(IServiceModelGenerationEnvironment generationEnvironment) : IGeneratePolicy<MethodIdentity>
{
    public virtual bool Used(Type baseDomainType, MethodIdentity identity)
    {
        var wrappedDomainType = generationEnvironment.MetadataProxyProvider.Wrap(baseDomainType);

        var allowedSingleDTO = () => wrappedDomainType.HasAttribute<BLLViewRoleAttribute>(attr => attr.Single.Contains((MainDTOType)identity.DTOType!.Value));
        var allowedCollectionDTO = () => wrappedDomainType.HasAttribute<BLLViewRoleAttribute>(attr => attr.Collection.Contains((MainDTOType)identity.DTOType!.Value));

        Func<ProjectionFilterTargets, bool> allowedProjectionFilter = target => wrappedDomainType.HasAttribute<ProjectionFilterAttribute>(attr => attr.FilterType == identity.ModelType && attr.Target.HasFlag(target));

        if (identity.DTOType == ViewDTOType.VisualDTO && !wrappedDomainType.HasVisualIdentityProperties())
        {
            return false;
        }
        else if (identity == MethodIdentityType.Save)
        {
            return wrappedDomainType.HasAttribute<BLLSaveRoleAttribute>(attr => attr.SaveType.HasFlag(BLLSaveType.Save) && attr.CountType.HasFlag(CountType.Single));
        }
        else if (identity == MethodIdentityType.SaveMany)
        {
            return wrappedDomainType.HasAttribute<BLLSaveRoleAttribute>(attr => attr.SaveType.HasFlag(BLLSaveType.Save) && attr.CountType.HasFlag(CountType.Many));
        }
        else if (identity == MethodIdentityType.Update)
        {
            return wrappedDomainType.HasAttribute<BLLSaveRoleAttribute>(attr => attr.SaveType.HasFlag(BLLSaveType.Update) && attr.CountType.HasFlag(CountType.Single));
        }
        else if (identity == MethodIdentityType.GetWithExtended)
        {
            return identity.ModelType.GetDirectMode().HasFlag(DirectMode.Out);
        }
        else if (identity == MethodIdentityType.SaveByModel)
        {
            return identity.ModelType.GetDirectMode().HasFlag(DirectMode.In);
        }
        else if (identity == MethodIdentityType.Remove)
        {
            return wrappedDomainType.HasAttribute<BLLRemoveRoleAttribute>(attr => attr.CountType.HasFlag(CountType.Single));
        }
        else if (identity == MethodIdentityType.RemoveMany)
        {
            return wrappedDomainType.HasAttribute<BLLRemoveRoleAttribute>(attr => attr.CountType.HasFlag(CountType.Many));
        }
        else if (identity == MethodIdentityType.GetPropertyRevisions
                 || identity == MethodIdentityType.GetPropertyRevisionByDateRange
                 || identity == MethodIdentityType.GetRevisions)
        {
            return wrappedDomainType.HasAttribute<BLLViewRoleAttribute>();
        }
        else if (identity == MethodIdentityType.GetRevision)
        {
            return allowedSingleDTO();
        }
        else if (identity == MethodIdentityType.AddAttachment
                 || identity == MethodIdentityType.GetAttachment
                 || identity == MethodIdentityType.RemoveAttachment)
        {
            return wrappedDomainType.HasAttribute<BLLViewRoleAttribute>();
        }
        else if (identity == MethodIdentityType.GetFileContainer)
        {
            return wrappedDomainType.HasAttribute<BLLViewRoleAttribute>();
        }
        else if (identity == MethodIdentityType.IntegrationSave)
        {
            return wrappedDomainType.HasAttribute<BLLIntegrationSaveRoleAttribute>(attr => attr.CountType.HasFlag(CountType.Single));
        }
        else if (identity == MethodIdentityType.IntegrationSaveMany)
        {
            return wrappedDomainType.HasAttribute<BLLIntegrationSaveRoleAttribute>(attr => attr.CountType.HasFlag(CountType.Many));
        }
        else if (identity == MethodIdentityType.IntegrationSaveByModel)
        {
            return true;
        }
        else if (identity == MethodIdentityType.IntegrationRemove)
        {
            return wrappedDomainType.HasAttribute<BLLIntegrationRemoveRoleAttribute>();
        }
        else if (identity == MethodIdentityType.GetChangeModel || identity == MethodIdentityType.GetMassChangeModel)
        {
            return identity.ModelType.GetDirectMode().HasFlag(DirectMode.Out);
        }
        else if (identity == MethodIdentityType.Change || identity == MethodIdentityType.MassChange)
        {
            return identity.ModelType.GetDirectMode().HasFlag(DirectMode.In);
        }
        else if (identity == MethodIdentityType.HasAccess || identity == MethodIdentityType.CheckAccess)
        {
            return false;
        }
        else if (identity == MethodIdentityType.Create)
        {
            return true;
        }
        else if (identity == MethodIdentityType.GetWithFormat)
        {
            return true;
        }
        else if (identity == MethodIdentityType.GetSingleByName)
        {
            return allowedSingleDTO();
        }
        else if (identity == MethodIdentityType.GetSingleByCode)
        {
            return allowedSingleDTO();
        }
        else if (identity == MethodIdentityType.GetSingleByIdentity)
        {
            if (wrappedDomainType.IsProjection())
            {
                return true;
            }
            else
            {
                return allowedSingleDTO();
            }
        }
        else if (identity == MethodIdentityType.GetSingleByFilter)
        {
            if (wrappedDomainType.IsProjection())
            {
                return allowedProjectionFilter(ProjectionFilterTargets.Single);
            }
            else
            {
                return allowedSingleDTO();
            }
        }
        else if (identity == MethodIdentityType.GetTreeByOperation || identity == MethodIdentityType.GetListByOperation)
        {
            return false;
        }
        else if (identity == MethodIdentityType.GetODataListByQueryString)
        {
            if (identity.DTOType == ViewDTOType.ProjectionDTO)
            {
                return wrappedDomainType.IsProjection();
            }
            else
            {
                return allowedCollectionDTO();
            }
        }
        else if (identity == MethodIdentityType.GetListByFilter)
        {
            if (wrappedDomainType.IsProjection())
            {
                return allowedProjectionFilter(ProjectionFilterTargets.Collection);
            }
            else
            {
                return allowedCollectionDTO();
            }
        }
        else if (identity == MethodIdentityType.GetListByIdents || identity == MethodIdentityType.GetList)
        {
            return allowedCollectionDTO();
        }
        else if (identity == MethodIdentityType.GetODataListByQueryStringWithFilter)
        {
            if (wrappedDomainType.IsProjection())
            {
                return allowedProjectionFilter(ProjectionFilterTargets.OData);
            }
            else
            {
                return allowedCollectionDTO();
            }
        }
        else if (identity == MethodIdentityType.GetODataTreeByQueryStringWithFilter)
        {
            if (wrappedDomainType.IsProjection())
            {
                return allowedProjectionFilter(ProjectionFilterTargets.ODataTree);
            }
            else
            {
                return allowedCollectionDTO();
            }
        }
        else if (identity == MethodIdentityType.GetODataListByQueryStringWithOperation
                 || identity == MethodIdentityType.GetODataTreeByQueryStringWithOperation)
        {
            return false;
        }
        else
        {
            return false;
        }
    }
}
