using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLLCoreGenerator;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;
using Framework.Projection;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

/// <summary>
/// Стандартная политика для управления генерацией фасадных методов
/// </summary>
public class DefaultServiceGeneratePolicy(IGenerationEnvironment generationEnvironment) : IGeneratePolicy<MethodIdentity>
{
    public virtual bool Used(Type domainType, MethodIdentity identity)
    {
        var extendedMetadata = generationEnvironment.ExtendedMetadata;

        var allowedSingleDTO = () => extendedMetadata.HasAttribute<BLLViewRoleAttribute>(domainType, attr => attr.Single.Contains((MainDTOType)identity.DTOType.Value));
        var allowedCollectionDTO = () => extendedMetadata.HasAttribute<BLLViewRoleAttribute>(domainType, attr => attr.Collection.Contains((MainDTOType)identity.DTOType.Value));

        Func<ProjectionFilterTargets, bool> allowedProjectionFilter = target => extendedMetadata.HasAttribute<ProjectionFilterAttribute>(domainType, attr => attr.FilterType == identity.ModelType && attr.Target.HasFlag(target));

        if (identity.DTOType == ViewDTOType.VisualDTO && !domainType.HasVisualIdentityProperties())
        {
            return false;
        }
        else if (identity == MethodIdentityType.Save)
        {
            return extendedMetadata.HasAttribute<BLLSaveRoleAttribute>(domainType, attr => attr.SaveType.HasFlag(BLLSaveType.Save) && attr.CountType.HasFlag(CountType.Single));
        }
        else if (identity == MethodIdentityType.SaveMany)
        {
            return extendedMetadata.HasAttribute<BLLSaveRoleAttribute>(domainType, attr => attr.SaveType.HasFlag(BLLSaveType.Save) && attr.CountType.HasFlag(CountType.Many));
        }
        else if (identity == MethodIdentityType.Update)
        {
            return extendedMetadata.HasAttribute<BLLSaveRoleAttribute>(domainType, attr => attr.SaveType.HasFlag(BLLSaveType.Update) && attr.CountType.HasFlag(CountType.Single));
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
            return extendedMetadata.HasAttribute<BLLRemoveRoleAttribute>(domainType, attr => attr.CountType.HasFlag(CountType.Single));
        }
        else if (identity == MethodIdentityType.RemoveMany)
        {
            return extendedMetadata.HasAttribute<BLLRemoveRoleAttribute>(domainType, attr => attr.CountType.HasFlag(CountType.Many));
        }
        else if (identity == MethodIdentityType.GetPropertyRevisions
                 || identity == MethodIdentityType.GetPropertyRevisionByDateRange
                 || identity == MethodIdentityType.GetRevisions)
        {
            return extendedMetadata.HasAttribute<BLLViewRoleAttribute>(domainType);
        }
        else if (identity == MethodIdentityType.GetRevision)
        {
            return allowedSingleDTO();
        }
        else if (identity == MethodIdentityType.AddAttachment
                 || identity == MethodIdentityType.GetAttachment
                 || identity == MethodIdentityType.RemoveAttachment)
        {
            return extendedMetadata.HasAttribute<BLLViewRoleAttribute>(domainType);
        }
        else if (identity == MethodIdentityType.GetFileContainer)
        {
            return extendedMetadata.HasAttribute<BLLViewRoleAttribute>(domainType);
        }
        else if (identity == MethodIdentityType.IntegrationSave)
        {
            return extendedMetadata.HasAttribute<BLLIntegrationSaveRoleAttribute>(domainType, attr => attr.CountType.HasFlag(CountType.Single));
        }
        else if (identity == MethodIdentityType.IntegrationSaveMany)
        {
            return extendedMetadata.HasAttribute<BLLIntegrationSaveRoleAttribute>(domainType, attr => attr.CountType.HasFlag(CountType.Many));
        }
        else if (identity == MethodIdentityType.IntegrationSaveByModel)
        {
            return true;
        }
        else if (identity == MethodIdentityType.IntegrationRemove)
        {
            return extendedMetadata.HasAttribute<BLLIntegrationRemoveRoleAttribute>(domainType);
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
            if (domainType.IsProjection())
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
            if (domainType.IsProjection())
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
                return domainType.IsProjection();
            }
            else
            {
                return allowedCollectionDTO();
            }
        }
        else if (identity == MethodIdentityType.GetListByFilter)
        {
            if (domainType.IsProjection())
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
            if (domainType.IsProjection())
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
            if (domainType.IsProjection())
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
