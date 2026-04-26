using Framework.BLL;
using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Persistent;
using Framework.Database;

using Microsoft.AspNetCore.Mvc;

using Anch.OData.Domain;
using SampleSystem.Domain.BU;
using SampleSystem.Generated.DTO;
using SelectOperationResultExtensions = Framework.BLL.OData.SelectOperationResultExtensions;

namespace SampleSystem.WebApiCore.Controllers.Main;

public partial class BusinessUnitController
{
    [HttpPost]
    public List<HierarchicalNode<BusinessUnitFullDTO, Guid>> GetFullBusinessUnitsTree() =>
        this.Evaluate(
            DBSessionMode.Read,
            evaluateData =>
            {
                var bll = evaluateData.Context.Logics.BusinessUnit;
                var tree = bll.GetTree(new DTOFetchRule<BusinessUnit>(ViewDTOType.FullDTO));
                return HierarchicalNodeExtensions.ChangeItem<BusinessUnit, BusinessUnitFullDTO, Guid>(tree, unit => LambdaHelper.ToFullDTO((BusinessUnit)unit, evaluateData.MappingService));
            });

    [HttpPost]
    public SelectOperationResult<HierarchicalNode<BusinessUnitFullDTO, Guid>> GetFullBusinessUnitsTreeByOData(string odataQueryString) =>
        this.Evaluate(
            DBSessionMode.Read,
            evaluateData =>
            {
                var bll = evaluateData.Context.Logics.BusinessUnit;
                var selectOperation = evaluateData.Context.SelectOperationParser.Parse<BusinessUnit>(odataQueryString);

                var odataTree = bll.GetTreeByOData(
                    selectOperation,
                    new DTOFetchRule<BusinessUnit>(ViewDTOType.FullDTO));

                return SelectOperationResultExtensions.ChangeItem<BusinessUnit, BusinessUnitFullDTO, Guid>(odataTree, x => LambdaHelper.ToFullDTO((BusinessUnit)x, evaluateData.MappingService));
            });
}
