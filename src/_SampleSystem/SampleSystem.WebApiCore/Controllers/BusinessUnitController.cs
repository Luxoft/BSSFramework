using Framework.BLL;
using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Persistent;
using Framework.BLL.OData;
using Framework.Database;

using Microsoft.AspNetCore.Mvc;

using OData.Domain;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

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
                return tree.ChangeItem(unit => unit.ToFullDTO(evaluateData.MappingService));
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

                return odataTree.ChangeItem(x => x.ToFullDTO(evaluateData.MappingService));
            });
}
