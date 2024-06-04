﻿// ------------------------------------------------------------------------------
/// <auto-generated>
/// This code was generated by a tool.
///
/// Changes to this file may cause incorrect behavior and will be lost if
/// the code is regenerated.
/// </auto-generated>
// ------------------------------------------------------------------------------

// tslint:disable
/* eslint-disable */

import { Guid, Convert, SimpleObject, SimpleDate, ObservableSimpleObject, ObservableSimpleDate } from 'luxite/system';
import * as async from 'luxite/async';
import { OData } from 'luxite/framework/odata';
import { Environment } from 'luxite/environment';
import { Core } from 'luxite/framework/framework';
import * as dto from 'luxite/report-generation/dto/all';
import { Stream } from '../../mocked-system';

export let doHealthCheckAsyncFunc = _doHealthCheck();
export let getReportParameterValuePositionAsyncFunc = _getReportParameterValuePosition();
export let getReportParameterValuePositionByTypeNameAsyncFunc = _getReportParameterValuePositionByTypeName();
export let getReportParameterValuePositionsAsyncFunc = _getReportParameterValuePositions();
export let getReportParameterValuePositionsByTypeNameAsyncFunc = _getReportParameterValuePositionsByTypeName();
export let getRichReportAsyncFunc = _getRichReport();
export let getRichReportGenerationRequestModelAsyncFunc = _getRichReportGenerationRequestModel();
export let getSimpleReportFiltersAsyncFunc = _getSimpleReportFilters();
export let getSimpleReportParametersAsyncFunc = _getSimpleReportParameters();
export let getSimpleReportParameterValuesAsyncFunc = _getSimpleReportParameterValues();
export let getSimpleReportParameterValuesByTypeNameAsyncFunc = _getSimpleReportParameterValuesByTypeName();
export let getSimpleReportPropertiesAsyncFunc = _getSimpleReportProperties();
export let getSimpleReportsAsyncFunc = _getSimpleReports();
export let getTypeMetadatasAsyncFunc = _getTypeMetadatas();
export let removeReportAsyncFunc = _removeReport();
export let saveReportAsyncFunc = _saveReport();

function _doHealthCheck(): async.SimpleAsyncFunc1<string> {
    return new async.SimpleAsyncFunc1(() => {
        let baseParameters = {};
        let service = Environment.current.context.facadeFactory.createReportSimpleService<string>();
        return service.getData('SampleSystemGenericReport/DoHealthCheck', baseParameters);
    });
}

    function _getReportParameterValuePosition(): async.SimpleAsyncFunc2<dto.ReportParameterValueStrictDTO, number> {
        return new async.SimpleAsyncFunc2((parameterValueStrictDTO: dto.ReportParameterValueStrictDTO) => {
            let baseParameters = parameterValueStrictDTO.toNativeJson();
            let service = Environment.current.context.facadeFactory.createReportSimpleService<number>();
            return service.getData('SampleSystemGenericReport/GetReportParameterValuePosition', baseParameters);
        });
    }

    function _getReportParameterValuePositionByTypeName(): async.SimpleAsyncFunc4<string, Guid, string, number> {
        return new async.SimpleAsyncFunc4((typeName: string, id: Guid, odataQueryString: string) => {
            let baseParameters = {typeName : typeName, id : id, odataQueryString : odataQueryString};
            let service = Environment.current.context.facadeFactory.createReportSimpleService<number>();
            return service.getData('SampleSystemGenericReport/GetReportParameterValuePositionByTypeName', baseParameters);
        });
    }

    function _getReportParameterValuePositions(): async.AsyncFunc3<Array<dto.ReportParameterValueStrictDTO>, Array<number>, Array<number>, number, number> {
        return new async.AsyncFunc3((parameterValuesDto: Array<dto.ReportParameterValueStrictDTO>) => {
            let baseParameters = parameterValuesDto;
            let service = Environment.current.context.facadeFactory.createReportService<Array<number>, Array<number>, number, number>();
            return service.getData('SampleSystemGenericReport/GetReportParameterValuePositions', {plain : SimpleObject, observable : ObservableSimpleObject}, baseParameters);
        });
    }

    function _getReportParameterValuePositionsByTypeName(): async.AsyncFunc5<string, Array<Guid>, string, Array<number>, Array<number>, number, number> {
        return new async.AsyncFunc5((typeName: string, ids: Array<Guid>, odataQueryString: string) => {
            let baseParameters = {typeName : typeName, ids : ids, odataQueryString : odataQueryString};
            let service = Environment.current.context.facadeFactory.createReportService<Array<number>, Array<number>, number, number>();
            return service.getData('SampleSystemGenericReport/GetReportParameterValuePositionsByTypeName', {plain : SimpleObject, observable : ObservableSimpleObject}, baseParameters);
        });
    }

    function _getRichReport(): async.AsyncFunc3<dto.ReportIdentityDTO, dto.ReportRichDTO, dto.ReportObservableRichDTO, dto.ReportRichDTO, dto.ReportObservableRichDTO> {
        return new async.AsyncFunc3((reportIdentity: dto.ReportIdentityDTO) => {
            let baseParameters = reportIdentity;
            let service = Environment.current.context.facadeFactory.createReportService<dto.ReportRichDTO, dto.ReportObservableRichDTO, dto.ReportRichDTO, dto.ReportObservableRichDTO>();
            return service.getData('SampleSystemGenericReport/GetRichReport', {plain : dto.ReportRichDTO, observable : dto.ReportObservableRichDTO}, baseParameters);
        });
    }

    function _getRichReportGenerationRequestModel(): async.AsyncFunc3<Guid, dto.ReportGenerationRequestModelRichDTO, dto.ReportGenerationRequestModelObservableRichDTO, dto.ReportGenerationRequestModelRichDTO, dto.ReportGenerationRequestModelObservableRichDTO> {
        return new async.AsyncFunc3((reportIdentity: Guid) => {
            let baseParameters = reportIdentity;
            let service = Environment.current.context.facadeFactory.createReportService<dto.ReportGenerationRequestModelRichDTO, dto.ReportGenerationRequestModelObservableRichDTO, dto.ReportGenerationRequestModelRichDTO, dto.ReportGenerationRequestModelObservableRichDTO>();
            return service.getData('SampleSystemGenericReport/GetRichReportGenerationRequestModel', {plain : dto.ReportGenerationRequestModelRichDTO, observable : dto.ReportGenerationRequestModelObservableRichDTO}, baseParameters);
        });
    }

    function _getSimpleReportFilters(): async.AsyncFunc3<Guid, Array<dto.ReportFilterSimpleDTO>, Array<dto.ReportFilterObservableSimpleDTO>, dto.ReportFilterSimpleDTO, dto.ReportFilterObservableSimpleDTO> {
        return new async.AsyncFunc3((reportIdentity: Guid) => {
            let baseParameters = reportIdentity;
            let service = Environment.current.context.facadeFactory.createReportService<Array<dto.ReportFilterSimpleDTO>, Array<dto.ReportFilterObservableSimpleDTO>, dto.ReportFilterSimpleDTO, dto.ReportFilterObservableSimpleDTO>();
            return service.getData('SampleSystemGenericReport/GetSimpleReportFilters', {plain : dto.ReportFilterSimpleDTO, observable : dto.ReportFilterObservableSimpleDTO}, baseParameters);
        });
    }

    function _getSimpleReportParameters(): async.AsyncFunc3<Guid, Array<dto.ReportParameterSimpleDTO>, Array<dto.ReportParameterObservableSimpleDTO>, dto.ReportParameterSimpleDTO, dto.ReportParameterObservableSimpleDTO> {
        return new async.AsyncFunc3((reportIdentity: Guid) => {
            let baseParameters = reportIdentity;
            let service = Environment.current.context.facadeFactory.createReportService<Array<dto.ReportParameterSimpleDTO>, Array<dto.ReportParameterObservableSimpleDTO>, dto.ReportParameterSimpleDTO, dto.ReportParameterObservableSimpleDTO>();
            return service.getData('SampleSystemGenericReport/GetSimpleReportParameters', {plain : dto.ReportParameterSimpleDTO, observable : dto.ReportParameterObservableSimpleDTO}, baseParameters);
        });
    }

    function _getSimpleReportParameterValues(): async.AsyncFunc4<dto.ReportParameterIdentityDTO, string, OData.SelectOperationResult<dto.ReportParameterValueSimpleDTO>, OData.SelectOperationResult<dto.ReportParameterValueObservableSimpleDTO>, dto.ReportParameterValueSimpleDTO, dto.ReportParameterValueObservableSimpleDTO> {
        return new async.AsyncFunc4((identity: dto.ReportParameterIdentityDTO, odataQueryString: string) => {
            let baseParameters = {identity : identity, odataQueryString : odataQueryString};
            let service = Environment.current.context.facadeFactory.createReportService<OData.SelectOperationResult<dto.ReportParameterValueSimpleDTO>, OData.SelectOperationResult<dto.ReportParameterValueObservableSimpleDTO>, dto.ReportParameterValueSimpleDTO, dto.ReportParameterValueObservableSimpleDTO>();
            return service.getData('SampleSystemGenericReport/GetSimpleReportParameterValues', {plain : dto.ReportParameterValueSimpleDTO, observable : dto.ReportParameterValueObservableSimpleDTO}, baseParameters);
        });
    }

    function _getSimpleReportParameterValuesByTypeName(): async.AsyncFunc4<string, string, OData.SelectOperationResult<dto.ReportParameterValueSimpleDTO>, OData.SelectOperationResult<dto.ReportParameterValueObservableSimpleDTO>, dto.ReportParameterValueSimpleDTO, dto.ReportParameterValueObservableSimpleDTO> {
        return new async.AsyncFunc4((typeName: string, odataQueryString: string) => {
            let baseParameters = {typeName : typeName, odataQueryString : odataQueryString};
            let service = Environment.current.context.facadeFactory.createReportService<OData.SelectOperationResult<dto.ReportParameterValueSimpleDTO>, OData.SelectOperationResult<dto.ReportParameterValueObservableSimpleDTO>, dto.ReportParameterValueSimpleDTO, dto.ReportParameterValueObservableSimpleDTO>();
            return service.getData('SampleSystemGenericReport/GetSimpleReportParameterValuesByTypeName', {plain : dto.ReportParameterValueSimpleDTO, observable : dto.ReportParameterValueObservableSimpleDTO}, baseParameters);
        });
    }

    function _getSimpleReportProperties(): async.AsyncFunc3<Guid, Array<dto.ReportPropertySimpleDTO>, Array<dto.ReportPropertyObservableSimpleDTO>, dto.ReportPropertySimpleDTO, dto.ReportPropertyObservableSimpleDTO> {
        return new async.AsyncFunc3((reportIdentity: Guid) => {
            let baseParameters = reportIdentity;
            let service = Environment.current.context.facadeFactory.createReportService<Array<dto.ReportPropertySimpleDTO>, Array<dto.ReportPropertyObservableSimpleDTO>, dto.ReportPropertySimpleDTO, dto.ReportPropertyObservableSimpleDTO>();
            return service.getData('SampleSystemGenericReport/GetSimpleReportProperties', {plain : dto.ReportPropertySimpleDTO, observable : dto.ReportPropertyObservableSimpleDTO}, baseParameters);
        });
    }

    function _getSimpleReports(): async.AsyncFunc3<string, OData.SelectOperationResult<dto.ReportSimpleDTO>, OData.SelectOperationResult<dto.ReportObservableSimpleDTO>, dto.ReportSimpleDTO, dto.ReportObservableSimpleDTO> {
        return new async.AsyncFunc3((odataQueryString: string) => {
            let baseParameters = odataQueryString;
            let service = Environment.current.context.facadeFactory.createReportService<OData.SelectOperationResult<dto.ReportSimpleDTO>, OData.SelectOperationResult<dto.ReportObservableSimpleDTO>, dto.ReportSimpleDTO, dto.ReportObservableSimpleDTO>();
            return service.getData('SampleSystemGenericReport/GetSimpleReports', {plain : dto.ReportSimpleDTO, observable : dto.ReportObservableSimpleDTO}, baseParameters);
        });
    }

    function _getTypeMetadatas(): async.AsyncFunc2<Array<dto.TypeMetadata>, Array<dto.TypeMetadata>, dto.TypeMetadata, dto.TypeMetadata> {
        return new async.AsyncFunc2(() => {
            let baseParameters = {};
            let service = Environment.current.context.facadeFactory.createReportService<Array<dto.TypeMetadata>, Array<dto.TypeMetadata>, dto.TypeMetadata, dto.TypeMetadata>();
            return service.getData('SampleSystemGenericReport/GetTypeMetadatas', {plain : dto.TypeMetadata, observable : dto.TypeMetadata}, baseParameters);
        });
    }

    function _removeReport(): async.SimpleAsyncFunc2<dto.ReportIdentityDTO, void> {
        return new async.SimpleAsyncFunc2((reportIdent: dto.ReportIdentityDTO) => {
            let baseParameters = reportIdent;
            let service = Environment.current.context.facadeFactory.createReportSimpleService<void>();
            return service.getData('SampleSystemGenericReport/RemoveReport', baseParameters);
        });
    }

    function _saveReport(): async.AsyncFunc3<dto.ReportStrictDTO, dto.ReportIdentityDTO, dto.ReportObservableIdentityDTO, dto.ReportIdentityDTO, dto.ReportObservableIdentityDTO> {
        return new async.AsyncFunc3((reportStrict: dto.ReportStrictDTO) => {
            let baseParameters = reportStrict.toNativeJson();
            let service = Environment.current.context.facadeFactory.createReportService<dto.ReportIdentityDTO, dto.ReportObservableIdentityDTO, dto.ReportIdentityDTO, dto.ReportObservableIdentityDTO>();
            return service.getData('SampleSystemGenericReport/SaveReport', {plain : dto.ReportIdentityDTO, observable : dto.ReportObservableIdentityDTO}, baseParameters);
        });
    }
