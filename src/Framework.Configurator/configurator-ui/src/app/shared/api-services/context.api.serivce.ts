import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, shareReplay } from 'rxjs';
import { IRoleContext } from 'src/app/rights/components/principals/components/grant-rights-dialog/grant-rights-dialog.models';
import { IEntity } from 'src/app/rights/components/principals/components/principal.models';

@Injectable()
export class ContextsApiService {
  constructor(private readonly http: HttpClient) {}

  private readonly contexts$ = this.http.get<IRoleContext[]>('api/contexts').pipe(shareReplay(1));
  public getContexts(): Observable<IRoleContext[]> {
    return this.contexts$;
  }

  public getEntities(unitId: string, searchToken: string): Observable<IEntity[]> {
    return this.http.get<IEntity[]>(`api/context/${unitId}/entities?`, { params: { searchToken } });
  }
}
