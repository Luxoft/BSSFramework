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
import * as dto from '../dto/authorization.generated';
import * as mockdto from '../../mocked-dto';

export let getCurrentPrincipalAsyncFunc = _getCurrentPrincipal();
export let getSecurityOperationsAsyncFunc = _getSecurityOperations();

function _getCurrentPrincipal(): async.AsyncFunc2<dto.PrincipalFullDTO, dto.PrincipalObservableFullDTO, dto.PrincipalFullDTO, dto.PrincipalObservableFullDTO> {
    return new async.AsyncFunc2(() => {
        let baseParameters = {};
        let realParameters = baseParameters;
        let service = Environment.current.context.facadeFactory.createAuthService<dto.PrincipalFullDTO, dto.PrincipalObservableFullDTO, dto.PrincipalFullDTO, dto.PrincipalObservableFullDTO>();
        return service.getData('Principal/GetCurrentPrincipal', {plain : dto.PrincipalFullDTO, observable : dto.PrincipalObservableFullDTO}, realParameters);
    });
}

    function _getSecurityOperations(): async.AsyncFunc2<Array<string>, Array<string>, string, string> {
        return new async.AsyncFunc2(() => {
            let baseParameters = {};
            let realParameters = baseParameters;
            let service = Environment.current.context.facadeFactory.createAuthService<Array<string>, Array<string>, string, string>();
            return service.getData('Operation/GetSecurityOperations', {plain : SimpleObject, observable : ObservableSimpleObject}, realParameters);
        });
    }

