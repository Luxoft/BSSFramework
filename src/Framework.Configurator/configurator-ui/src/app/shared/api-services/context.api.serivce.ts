import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IRoleContext } from 'src/app/rights/components/principals/components/grant-rights-dialog/grant-rights-dialog.models';
import { IEntity } from 'src/app/rights/components/principals/components/view-principal-dialog/view-principal-dialog.component';

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
