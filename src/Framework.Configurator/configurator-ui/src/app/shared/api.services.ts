import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  IEntity,
  IPrincipalDetails,
} from '../rights/components/principals/components/view-principal-dialog/view-principal-dialog.component';
import { Observable } from 'rxjs';
import { IGrantedRight, IRoleContext } from '../rights/components/principals/components/grant-rights-dialog/grant-rights-dialog.models';

@Injectable()
export class PrincipalApiService {
  constructor(private readonly http: HttpClient) {}

  public getPrincipal(principalId: string): Observable<IPrincipalDetails> {
    return this.http.get<IPrincipalDetails>(`api/principal/${principalId}`);
  }

  public savePermissions(principalId: string, permissions: IGrantedRight[]): Observable<object> {
    return this.http.post(`api/principal/${principalId}/permissions`, permissions);
  }
}

@Injectable()
export class ContextsApiService {
  constructor(private readonly http: HttpClient) {}

  public getContexts(): Observable<IRoleContext[]> {
    return this.http.get<IRoleContext[]>('api/contexts');
  }

  public getEntities(unitId: string, searchToken: string): Observable<IEntity[]> {
    return this.http.get<IEntity[]>(`api/context/${unitId}/entities?`, { params: { searchToken } });
  }
}
