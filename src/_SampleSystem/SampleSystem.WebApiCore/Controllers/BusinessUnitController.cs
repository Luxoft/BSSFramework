﻿using Framework.DomainDriven;
using Framework.OData;
using Framework.Persistent;
using Framework.Transfering;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.WebApiCore.Controllers.Main;

public partial class BusinessUnitController
{
    [HttpPost]
    public List<HierarchicalNode<BusinessUnitFullDTO, Guid>> GetFullBusinessUnitsTree()
    {
        return this.Evaluate(
                             DBSessionMode.Read,
                             evaluateData =>
                             {
                                 var bll = evaluateData.Context.Logics.BusinessUnit;
                                 var tree = bll.GetTree(evaluateData.Context.FetchService.GetContainer<BusinessUnit>(ViewDTOType.FullDTO));
                                 return tree.ChangeItem(unit => unit.ToFullDTO(evaluateData.MappingService));
                             });
    }

    [HttpPost]
    public SelectOperationResult<HierarchicalNode<BusinessUnitFullDTO, Guid>> GetFullBusinessUnitsTreeByOData(string odataQueryString)
    {
        return this.Evaluate(
                             DBSessionMode.Read,
                             evaluateData =>
                             {
                                 var bll = evaluateData.Context.Logics.BusinessUnit;
                                 var selectOperation = evaluateData.Context.SelectOperationParser.Parse(odataQueryString);
                                 var typedSelectOperation = evaluateData.Context.StandartExpressionBuilder.ToTyped<BusinessUnit>(selectOperation);

                                 var odataTree = bll.GetTreeByOData(
                                                                    typedSelectOperation,
                                                                    evaluateData.Context.FetchService.GetContainer<BusinessUnit>(ViewDTOType.FullDTO));

                                 return odataTree.ChangeItem(x => x.ToFullDTO(evaluateData.MappingService));
                             });
    }

    [HttpPost]
    public List<HierarchicalNode<BusinessUnitFullDTO, Guid>> TestPeriod(Framework.Core.Period period)
    {
        return null;
    }
}
